// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Models;
using Infrastructure.Persistence;
using Serilog;

namespace Api.Handlers;

/// <summary>
/// MigrationsHandler
/// </summary>
public static class MigrationsHandler
{
    /// <summary>
    /// ApplyMigration
    /// </summary>
    /// <param name="app"></param>
    /// <param name="environment"></param>
    /// <param name="appSetting"></param>
    /// <returns></returns>
    public static async Task ApplyMigration(
        IApplicationBuilder app, IWebHostEnvironment environment, AppSetting appSetting)
    {
        var logger = Log.ForContext(typeof(MigrationsHandler));

        if (!appSetting.DatabaseSettings.Migrations && !appSetting.DatabaseSettings.SeedData)
        {
            logger.Information("No Migration Started");
            return;
        }

        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
       

        try
        {
            if (serviceScope != null)
            {
                var initializer = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

                if (appSetting.DatabaseSettings.Migrations)
                {
                    logger.Information("Migration Database");

                    await initializer.InitializeAsync().ConfigureAwait(false);
                }

                if (appSetting.DatabaseSettings.SeedData)
                {
                    logger.Information("Seeding Database");

                    await initializer.SeedAsync().ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while migrating the database.");
        }
    }
}

/// <summary>
/// UseMigrationsHandlerExtensions
/// </summary>
public static class UseMigrationsHandlerExtensions
{
    /// <summary>
    /// UseMigrationsHandler
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="environment"></param>
    /// <param name="appSetting"></param>
    public static void UseMigrationsHandler(
        this IApplicationBuilder builder, IWebHostEnvironment environment, AppSetting appSetting)
    {
        MigrationsHandler.ApplyMigration(builder, environment, appSetting).ConfigureAwait(false);
    }
}
