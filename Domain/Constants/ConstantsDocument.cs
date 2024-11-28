// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Domain.Constants;

/// <summary>
/// ConstantsDocument
/// </summary>
public abstract class ConstantsDocument
{
    /// <summary>
    /// Excel File Extensions
    /// </summary>
    public static readonly IReadOnlyList<string> ExcelFileExtensions = new List<string>
    {
        ".xls",
        ".xlsx"
    };

    /// <summary>
    /// FileExtensions
    /// </summary>
    public static readonly IReadOnlyList<string> FileExtensions = new List<string>
    {
        ".xls",
        ".xlsx",
        ".csv",
        ".doc",
        ".docx",
        ".pdf",
        ".jpg",
        ".jpeg",
        ".png",
        ".tiff",
        ".heif",
        ".bmp"
    };

    /// <summary>
    /// UtilityIso
    /// </summary>
    public const string UtilityIso = "ISO";
}
