// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsExcelReport
/// </summary>
public abstract class ConstantsExcelReport
{
    /// <summary>
    /// ExcelWorksheet
    /// </summary>
    public const string Worksheet = "Summary Report";


    /// <summary>
    /// ExcelFileNameSecond
    /// </summary>
    public const string FileNameSecond = "_REPORT_";

    /// <summary>
    /// ExcelExt
    /// </summary>
    public const string Ext = ".xlsx";

    /// <summary>
    /// DateFormat
    /// </summary>
    public const string DateFormat = "yyyy/MM/dd";

    /// <summary>
    /// TimeFormat
    /// </summary>
    public const string TimeFormat = "HH:mm:ss";

    /// <summary>
    /// Blank
    /// </summary>
    public const string Blank = "-";

    /// <summary>
    /// CustomRuleAsciiAlphabet (Actual ASCII alphabet start from 65)
    /// </summary>
    public const int CustomRuleAsciiAlphabet = 64;
}
