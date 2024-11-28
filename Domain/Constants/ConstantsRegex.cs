// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsRegex
/// </summary>
public abstract class ConstantsRegex
{
    /// <summary>
    /// RegexPattern
    /// </summary>
    public const string Pattern = @"^[{pattern}]+$";

    /// <summary>
    /// RegexChar
    /// </summary>
    public const string Char = "a-zA-Z";

    /// <summary>
    /// RegexNumeric
    /// </summary>
    public const string Numeric = "0-9";

    /// <summary>
    /// RegexSymbol
    /// </summary>
    public const string Symbol = @"\&()@_:;/.-%";
}
