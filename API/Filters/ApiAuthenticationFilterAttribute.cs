// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Common.Models;

namespace Api.Filters;

/// <summary>
/// ApiAuthenticationFilterAttribute
/// </summary>
public class ApiAuthenticationFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// OnActionExecutionAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiAuthenticationFilterAttribute>>();
        var appSetting = context.HttpContext.RequestServices.GetRequiredService<AppSetting>();
        var environment = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        try
        {
            var isAuth = !environment.IsDevelopment() || appSetting.IsEnableAuth;
            if (!isAuth)
            {
                await next().ConfigureAwait(false);
            }
            else
            {
                var auth = await CheckAuthAsync(context.HttpContext).ConfigureAwait(false);
                if (!auth.Succeeded)
                {
                    await context.HttpContext.ChallengeAsync().ConfigureAwait(false);
                    return;
                }

                await next().ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            logger.LogError("error authentication token {Message}", e.Message);
            context.Result = new UnauthorizedResult();
        }
    }

    private static async Task<AuthenticateResult> CheckAuthAsync(HttpContext context)
    {
        var auth = context.RequestServices.GetRequiredService<IAuthenticationService>();
        return await auth.AuthenticateAsync(context, scheme: JwtBearerDefaults.AuthenticationScheme).ConfigureAwait(false);
    }
}
