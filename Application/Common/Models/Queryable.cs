// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#region

using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Application.Common.Extensions;
using Domain.Constants;

#endregion

namespace Application.Common.Models;

/// <summary>
/// Queryable
/// </summary>
public static class Queryable
{
    private static readonly ILogger Logger = AppLoggingExtensions.CreateLogger(nameof(Queryable));

    private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
    {
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "startswith", "StartsWith" },
        { "endswith", "EndsWith" },
        { "contains", "Contains" },
        { "doesnotcontain", "Contains" },
        { "==", "=" },
        { "!=", "!=" },
        { "<", "<" },
        { "<=", "<=" },
        { ">", ">" },
        { ">=", ">=" },
        { "_=", "StartsWith" },
        { "=_", "EndsWith" },
        { "@=", "Contains" },
        { "!@=", "Contains" }
    };

    private static Type _type;

    /// <summary>
    /// Query
    /// </summary>
    /// <param name="source"></param>
    /// <param name="queryModel"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Query<T>(this IQueryable<T> source, QueryModel queryModel)
    {
        ArgumentNullException.ThrowIfNull(source);

        _type = typeof(T);

        source = Filter(source, queryModel.GetFiltersParsed<T>() ?? []);

        source = Sort(source, queryModel.GetSortsParsed());

        if (queryModel.PageNumber < 1)
        {
            queryModel.PageNumber = ConstantsPagination.DefaultPageNumber;
        }

        if (queryModel.PageSize > 0)
        {
            source = Limit(
                source,
                queryModel.PageNumber ?? ConstantsPagination.DefaultPageNumber,
                queryModel.PageSize ?? ConstantsPagination.DefaultPageSize);
        }

        return source;
    }

    /// <summary>
    /// QueryWithoutLimit
    /// </summary>
    /// <param name="source"></param>
    /// <param name="queryModel"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> QueryWithoutLimit<T>(
        this IQueryable<T> source,
        QueryModel queryModel)
    {
        ArgumentNullException.ThrowIfNull(source);

        _type = typeof(T);

        source = Filter(source, queryModel.GetFiltersParsed<T>() ?? []);

        source = Sort(source, queryModel.GetSortsParsed());

        return source;
    }

    /// <summary>
    /// GlobalSearchFilter
    /// </summary>
    /// <param name="searchValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IList<FilterQuery> GlobalSearchFilter<T>(string searchValue)
    {
        var globalSearchFilters = new List<FilterQuery>();

        if (string.IsNullOrWhiteSpace(searchValue))
        {
            return globalSearchFilters;
        }

        var properties = typeof(T).GetProperties()
            .Where(p => p.PropertyType == typeof(string) || p.PropertyType.IsValueType)
            .Select(p => new { p.Name, p.PropertyType })
            .ToList();

        foreach (var field in properties)
        {
            if (field.PropertyType == typeof(string))
            {
                globalSearchFilters.Add(new FilterQuery
                {
                    Field = field.Name,
                    PropertyType = field.PropertyType,
                    Operator = "contains",
                    Value = searchValue,
                    Logic = "OR"
                });
            }
        }

        return globalSearchFilters;
    }

    /// <summary>
    /// Filter
    /// </summary>
    /// <param name="source"></param>
    /// <param name="queryModel"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Filter<T>(this IQueryable<T> source, QueryModel queryModel)
    {
        ArgumentNullException.ThrowIfNull(source);

        _type = typeof(T);

        source = Filter(source, queryModel.GetFiltersParsed<T>() ?? []);

        return source;
    }

    private static IQueryable<T> Filter<T>(IQueryable<T> source, IList<FilterQuery> filters)
    {
        if (filters == null || !filters.Any())
        {
            return source;
        }

        try
        {
            var whereClause = SwitchLogic(filters);

            if (!string.IsNullOrEmpty(whereClause))
            {
                var values = filters.Select(f => f.Value).ToArray();
                Logger.LogDebug("Applying filter: Type={Type}, Where={Where}, Values={Values}", _type, whereClause,
                    values);

                source = source.Where(whereClause, values);
            }
        }
        catch (Exception e)
        {
            Logger.LogWarning("Filtering failed: {Message}", e.Message);
        }

        return source;
    }

    private static string SwitchLogic(IList<FilterQuery> filter)
    {
        var where = string.Empty;
        for (var i = 0; i < filter.Count; i++)
        {
            var logic = filter[i].Logic ?? "AND";
            string f;

            if (logic.StartsWith('('))
            {
                f =
                    i == 0
                        ? $"({Transform(logic[1..], filter[i], i)}"
                        : $"{Transform(logic[1..] + " (", filter[i], i)}";
            }
            else if (logic.EndsWith(')'))
            {
                f = $"{Transform(logic[..^1], filter[i], i)})";
            }
            else
            {
                f = Transform(logic, filter[i], i);
            }

            where = $"{where} {f}";
        }

        return where;
    }

    private static IQueryable<T> Limit<T>(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var skip = pageSize * (pageNumber - 1);
        Logger.LogDebug("Skipping {Skip} records and taking {PageSize} records", skip, pageSize);

        return source.Skip(skip).Take(pageSize);
    }

    /// <summary>
    /// Transform
    /// </summary>
    /// <param name="logic"></param>
    /// <param name="filter"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private static string Transform(string logic, FilterQuery filter, int index)
    {
        if (
            filter.Value == null
            || string.IsNullOrEmpty(filter.Field)
            || string.IsNullOrEmpty(filter.Value.ToString())
        )
        {
            return null;
        }

        try
        {
            if (filter.Operator != null)
            {
                return TransformLogic(Operators[filter.Operator.ToLower()], logic, filter, index,
                    filter.Value.ToString());
            }
        }
        catch (Exception e)
        {
            Logger.LogWarning("Transformation failed for operator {Operator}: {Message}", filter.Operator, e.Message);
        }

        return null;
    }

    private static string TransformLogic(
        string comparison,
        string logic,
        FilterQuery filter,
        int index,
        string value)
    {
        if (filter.Operator == "doesnotcontain")
        {
            if (index > 0)
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} ({1} != null && !{1}.{2}(@{3}))",
                    logic,
                    filter.Field,
                    comparison,
                    index);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "({0} != null && !{0}.{1}(@{2}))",
                filter.Field,
                comparison,
                index);
        }

        if (comparison != "StartsWith" && comparison != "EndsWith" && comparison != "Contains")
        {
            if (int.TryParse(value, out _) || double.TryParse(value, out _) ||
                float.TryParse(value, out _) || bool.TryParse(value, out _) || Guid.TryParse(value, out _))
            {
                return index > 0
                    ? string.Format(CultureInfo.InvariantCulture, "{0} ({1} != null && {1} {2} @{3})", logic,
                        filter.Field, comparison, index)
                    : string.Format(CultureInfo.InvariantCulture, "({0} != null && {0} {1} @{2})", filter.Field,
                        comparison, index);
            }

            if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, out _))
            {
                return index > 0
                    ? string.Format(CultureInfo.InvariantCulture, "{0} ({1} {2} @{3})", logic, filter.Field, comparison,
                        index)
                    : string.Format(CultureInfo.InvariantCulture, "({0}{1} @{2})", filter.Field, comparison, index);
            }

            return index > 0
                ? string.Format(CultureInfo.InvariantCulture, "{0} ({1} != null && {1}.ToLower() {2} @{3}.ToLower())",
                    logic,
                    filter.Field, comparison, index)
                : string.Format(CultureInfo.InvariantCulture, "({0} != null && {0}.ToLower() {1} @{2}.ToLower())",
                    filter.Field,
                    comparison, index);
        }

        return index > 0
            ? string.Format(
                CultureInfo.InvariantCulture,
                "{0} ({1}.ToLower().{2}(@{3}.ToLower()))",
                logic,
                filter.Field,
                comparison,
                index)
            : string.Format(
                CultureInfo.InvariantCulture,
                "({0}.ToLower().{1}(@{2}.ToLower()))",
                filter.Field,
                comparison,
                index);
    }

    /// <summary>
    /// Sorting query by column
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sort"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static IQueryable<T> Sort<T>(this IQueryable<T> source, IReadOnlyCollection<Sort> sort)
    {
        if (sort == null || !sort.Any())
        {
            return source;
        }

        try
        {
            var ordering = string.Join(",", sort.Select(s => $"{s.Field} {s.Direction}"));
            Logger.LogDebug("Applying sorting: Type={Type}, Ordering={Ordering}", _type, ordering);

            return source.OrderBy(ordering);
        }
        catch (ParseException e)
        {
            Logger.LogWarning("Sorting failed for type {Type}: {Message}", _type, e.Message);
        }

        return source;
    }
}
