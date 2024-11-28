// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Constants;
using Newtonsoft.Json.Linq;

namespace Api.Middlewares;

/// <summary>
/// OverrideResponseHandlerMiddleware
/// </summary>
public class OverrideResponseHandlerMiddleware
{
    private readonly IRedisService _redisService;
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="OverrideResponseHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="redisService"></param>
    /// <param name="appSetting"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public OverrideResponseHandlerMiddleware(
        RequestDelegate next,
        IRedisService redisService,
        AppSetting appSetting,
        ILogger<OverrideResponseHandlerMiddleware> logger)
    {
        _next = next;
        _appSetting = appSetting;
        _redisService = redisService;
        _logger = logger;
    }

    /// <summary>
    /// InvokeAsync
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        _logger.LogDebug("Overriding Response");

        var watch = new Stopwatch();
        watch.Start();

        var originalBody = context.Response.Body;

        try
        {
            var policyName = string.Empty;

            var memStream = new MemoryStream();
            await using (memStream.ConfigureAwait(false))
            {
                context.Response.Body = memStream;

                await _next(context).ConfigureAwait(false);

                policyName = (string)context.Items["CurrentPolicyName"];
                var statusCode = context.Response.StatusCode;
                memStream.Position = 0;

                if (statusCode != 200 || !context.Response.ContentType!.Contains(ConstantsHeader.Json))
                {
                    await memStream.CopyToAsync(originalBody).ConfigureAwait(false);
                    return;
                }

                var responseBody = await new StreamReader(memStream)
                    .ReadToEndAsync()
                    .ConfigureAwait(false);

                watch.Stop();
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;

                context = await RedisCachingAsync(policyName, context, responseBody)
                    .ConfigureAwait(false);

                var buffer = Encoding
                    .UTF8
                    .GetBytes(ToJsonApi(statusCode, responseTimeForCompleteRequest, responseBody));

                context.Response.ContentLength = buffer.Length;

                var output = new MemoryStream(buffer);
                await using (output.ConfigureAwait(false))
                {
                    output.Position = 0;
                    await output.CopyToAsync(originalBody).ConfigureAwait(false);
                }
            }
        }
        catch
        {
            watch.Stop();

            throw;
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }

    private static string ToJsonApi(
        int statusCode,
        long responseTimeForCompleteRequest,
        string responseBody)
    {
        var json = JObject.Parse(responseBody);
        json["responseTime"] = responseTimeForCompleteRequest;
        if (json["status"] is not JObject status)
        {
            var stat = new JObject
            {
                { "code", statusCode },
                { "desc", ReasonPhrases.GetReasonPhrase(statusCode) }
            };
            json.Add("status", stat);
        }
        else
        {
            status["code"] = statusCode;
            status["desc"] = ReasonPhrases.GetReasonPhrase(statusCode);
        }

        return json.ToString();
    }

    private async Task<HttpContext> RedisCachingAsync(
        string policyName,
        HttpContext context,
        string responseBody)
    {
        var requestIfNoneMatch =
            context.Request.Headers[ConstantsHeader.IfNoneMatch].ToString();
        if (string.IsNullOrEmpty(requestIfNoneMatch))
        {
            return context;
        }

        var policy = IsCache(policyName);
        if (policy is not { IsCache: true })
        {
            return context;
        }

        var key = await _redisService
            .SaveSubAsync(ConstantsRedis.SubKeyHttpRequest, policy.Name, responseBody)
            .ConfigureAwait(false);
        context.Response.Headers[ConstantsHeader.ETag] = key;
        return context;
    }

    private Policy IsCache(string policy)
    {
        var policyList = _appSetting.Redis.Policy;
        return policyList.Count.Equals(0)
            ? null
            : policyList.SingleOrDefault(x => x.Name.ToLower().Equals(policy.ToLower()));
    }
}

/// <summary>
/// OverrideResponseMiddlewareExtensions
/// </summary>
public static class OverrideResponseMiddlewareExtensions
{
    /// <summary>
    /// UseOverrideResponseHandler
    /// </summary>
    /// <param name="builder"></param>
    public static void UseOverrideResponseHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<OverrideResponseHandlerMiddleware>();
    }
}
