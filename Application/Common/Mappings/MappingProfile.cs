// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using AutoMapper;

namespace Application.Common.Mappings;

/// <summary>
/// MappingProfile
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    /// <summary>
    /// ApplyMappingsFromAssembly
    /// </summary>
    /// <param name="assembly"></param>
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly
            .GetExportedTypes()
            .Where(
                t =>
                    t.GetInterfaces()
                        .Any(
                            i =>
                                i.IsGenericType
                                && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo =
                type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

            methodInfo?.Invoke(instance, [this]);
        }
    }
}
