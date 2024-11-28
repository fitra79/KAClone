// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Scrutor;

namespace Application;

/// <summary>
/// DependencyInjection
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// AddApplicationServices
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

    }
}
