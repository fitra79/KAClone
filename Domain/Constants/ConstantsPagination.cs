// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsPagination
/// </summary>
public abstract class ConstantsPagination
{
    /// <summary>
    /// DefaultPageSize
    /// </summary>
    public const int DefaultPageSize = 10;

    /// <summary>
    /// DefaultPageSize
    /// </summary>
    public const int DefaultPageNumber = 1;

    /// <summary>
    /// SyncDateFormat
    /// </summary>
    public const string SyncDateFormat = "yyyyMMddHHmmss";

    /// <summary>
    /// CustomRuleAsciiAlphabet (Actual ASCII alphabet start from 65)
    /// </summary>
    public const int CustomRuleAsciiAlphabet = 64;
}
