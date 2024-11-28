// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Api.Filters;

/// <summary>
/// ApiUserGroupCustomAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public abstract class ApiUserGroupCustomAttribute : Attribute
{
    /// <summary>
    /// Gets or sets group
    /// </summary>
    public string[] Group { get; set; }
}
