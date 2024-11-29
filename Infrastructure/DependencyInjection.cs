// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.Cache;
// using Infrastructure.Services.Messages;
using Quartz;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Seeders;
using Infrastructure.Persistence.SeederRunner;

namespace Infrastructure;

/// <summary>
/// DependencyInjection
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            );
            options.AddInterceptors(interceptor);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IDateTimeOffset, DateTimeOffsetService>();

        services.AddScoped<CitySeeder>(); // Add this line
        services.AddScoped<SeederRunner>(); // Ensure SeederRunner is also registered

        return services;
    }

}
