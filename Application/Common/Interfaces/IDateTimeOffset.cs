// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Interfaces;

/// <summary>
/// IDateTimeOffset
/// </summary>
public interface IDateTimeOffset
{
    /// <summary>
    /// Gets now
    /// </summary>
    /// <value></value>
    DateTimeOffset Now { get; }

    /// <summary>
    /// Gets utcNow
    /// </summary>
    /// <value></value>
    DateTimeOffset UtcNow { get; }
}
