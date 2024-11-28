// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using AutoMapper.QueryableExtensions;
using JsonApiSerializer.JsonApi;
using Microsoft.EntityFrameworkCore;
using Application.Common.Extensions;

namespace Application.Common.Mappings;

/// <summary>
/// MappingExtensions
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// PaginatedListAsync
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="meta"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static Task<DocumentRootJson<List<TDestination>>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, Meta meta, int pageNumber, int pageSize)
        => JsonApiExtensionPaginated.CreateAsync(queryable, meta, pageNumber, pageSize);

    /// <summary>
    /// ProjectToListAsync
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable, IConfigurationProvider configuration)
        => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
}
