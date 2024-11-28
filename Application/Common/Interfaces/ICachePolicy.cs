// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;
using MediatR;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface of cache policy implementation.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICachePolicy<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Gets the cache sliding expiration for the value.
    /// </summary>
    public DateTime? AbsoluteExpiration => null;

    /// <summary>
    /// Gets the cache sliding expiration for the value.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow => null;

    /// <summary>
    /// Gets the cache absolute expiration relative to now for the value.
    /// </summary>
    public TimeSpan? SlidingExpiration => null;

    /// <summary>Gets a cache key.</summary>
    /// <param name="request">A requested value.</param>
    /// <param name="customerCode"></param>
    /// <param name="attributes"></param>
    /// <returns>Cache Key</returns>
    string GetCacheKey(TRequest request, string customerCode, Dictionary<string, List<string>> attributes)
    {
        var value = new { request };
        var props = value.request.GetType()
            .GetProperties()
            .Select(x =>
            {
                var temp = x.GetValue(value.request, null);

                if (temp is List<string>)
                {
                    return $"{x.Name}:{string.Join(",", temp as List<string>)}";
                }

                return $"{x.Name}:{temp}";
            })
            .ToList();
        var attributeValues = new List<string>();

        foreach (var attribute in attributes)
        {
            attributeValues.Add($"{attribute.Key}:[{string.Join(",", attribute.Value)}]");
        }

        var attributeKey = new Guid(SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(string.Join(",", attributeValues)))
            .Take(16)
            .ToArray())
            .ToString();

        var key = typeof(TRequest).FullName;
        var requestKey = string.Join(",", props);
        var customerCodeKey = $"CustomerCode:{customerCode}";
        attributeKey = $"AttributeKey:{attributeKey}";

        return $"{key}{{{customerCodeKey},{attributeKey},{requestKey}}}";
    }
}
