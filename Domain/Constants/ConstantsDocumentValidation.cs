// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Domain.Constants;

/// <summary>
/// ConstantsDocumentValidation
/// </summary>
public abstract class ConstantsDocumentValidation
{
    /// <summary>
    /// DefaultTotalMaxProcess
    /// </summary>
    public const byte DefaultTotalMaxProcess = 25;

    /// <summary>
    /// Excel File Extensions
    /// </summary>
    public static readonly IReadOnlyList<string> ImageFileExtensions = new List<string>
    {
        ".jpg",
        ".jpeg",
        ".png"
    };

    /// <summary>
    /// ExcelFileExtensions
    /// </summary>
    public static readonly IReadOnlyList<string> ExcelFileExtensions = new List<string>
    {
        ".xls",
        ".xlsx",
        ".csv"
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
}
