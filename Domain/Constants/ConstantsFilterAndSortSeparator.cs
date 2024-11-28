// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsFilterAndSortSeparator
/// </summary>
public abstract class ConstantsFilterAndSortSeparator
{
    /// <summary>
    /// Comma Separator
    /// </summary>
    public const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

    /// <summary>
    /// Pipe Separator
    /// </summary>
    public const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

    /// <summary>
    /// Comma Separator
    /// </summary>
    public static readonly string[] Operators =
    {
        "!@=*",
        "!_=*",
        "!=*",
        "!@=",
        "!_=",
        "==*",
        "@=*",
        "_=*",
        "==",
        "!=",
        ">=",
        "<=",
        ">",
        "<",
        "@=",
        "_=",
        "="
    };
}
