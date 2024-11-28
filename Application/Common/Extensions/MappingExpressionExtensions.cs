// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;

namespace Application.Common.Extensions;

/// <summary>
/// MappingExpressionExtensions
/// </summary>
public static class MappingExpressionExtensions
{
    /// <summary>
    /// IgnoreAllMembers
    /// </summary>
    /// <param name="expr"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static IMappingExpression<TSource, TDestination> IgnoreAllMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expr)
    {
        var destinationType = typeof(TDestination);

        foreach (var property in destinationType.GetProperties())
            expr.ForMember(property.Name, opt => opt.Ignore());

        return expr;
    }
}
