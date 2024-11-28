// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Application.Common.Interfaces;
using Domain.Constants;

namespace Api.Middlewares;

/// <summary>
/// OverrideRequestHandlerMiddleware
/// </summary>
public class OverrideRequestHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IRedisService _redisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OverrideRequestHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="redisService"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public OverrideRequestHandlerMiddleware(
        RequestDelegate next,
        IRedisService redisService,
        ILogger<OverrideRequestHandlerMiddleware> logger)
    {
        _next = next;
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
        _logger.LogDebug("Overriding Request");

        if (context.Request.Path.StartsWithSegments("/api"))
        {
            var requestIfNoneMatch = context.Request.Headers[ConstantsHeader.IfNoneMatch].ToString() ?? "";

            if (!string.IsNullOrEmpty(requestIfNoneMatch))
            {
                var encodedEntity = await _redisService.GetAsync(requestIfNoneMatch).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(encodedEntity))
                {
                    const int code = (int)HttpStatusCode.NotModified;
                    context.Response.StatusCode = code;
                    return;
                }
            }
        }

        await _next(context).ConfigureAwait(false);
    }
}

/// <summary>
/// OverrideRequestMiddlewareExtensions
/// </summary>
public static class OverrideRequestMiddlewareExtensions
{
    /// <summary>
    /// UseOverrideRequestHandler
    /// </summary>
    /// <param name="builder"></param>
    public static void UseOverrideRequestHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<OverrideRequestHandlerMiddleware>();
    }
}
