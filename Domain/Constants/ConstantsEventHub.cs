// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Domain.Constants;

/// <summary>
/// ConstantsEventHub
/// </summary>
public abstract class ConstantsEventHub
{
    /// <summary>
    /// ConsumerList
    /// </summary>
    public static readonly IReadOnlyDictionary<string, (string, bool)> ConsumerJobList =
        new Dictionary<string, (string, bool)> { };
}
