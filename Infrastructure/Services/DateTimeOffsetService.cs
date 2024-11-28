// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Application.Common.Interfaces;

namespace Infrastructure.Services;

/// <summary>
/// DateTimeService
/// </summary>
public class DateTimeOffsetService : IDateTimeOffset
{
    /// <summary>
    /// Gets now
    /// </summary>
    public DateTimeOffset Now => DateTimeOffset.Now;

    /// <summary>
    /// Gets utcNow
    /// </summary>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
