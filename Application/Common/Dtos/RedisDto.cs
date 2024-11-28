// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Dtos;

/// <summary>
/// RedisDto
/// </summary>
public record RedisDto
{
    /// <summary>
    /// Gets or sets code
    /// </summary>
    /// <value></value>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets desc
    /// </summary>
    /// <value></value>
    public string Value { get; set; }
}
