// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Domain.Constants;

/// <summary>
/// ConstantsProcess
/// </summary>
public abstract class ConstantsProcess
{
    /// <summary>
    /// DefaultTotalMaxProcess
    /// </summary>
    public const byte DefaultTotalMaxProcess = 25;

    /// <summary>
    /// File Extensions
    /// </summary>
    public static readonly IReadOnlyList<string> FileExtensions = new List<string> { "csv", "xls", "xlsx" };

    /// <summary>
    /// CatEquipmentVHC
    /// </summary>
    public const string CatEquipmentVHC = "VHC001";

    /// <summary>
    /// CatEquipment
    /// </summary>
    public const string CatEquipment = "EQ001";

    /// <summary>
    /// ColorStagings
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> ColorStagings = new Dictionary<string, string>
    {
        { ConstantsSystem.StagingNameCreate, "#AAB0B5" },
        { ConstantsSystem.StagingNameAssign, "#AAB0B5" },
        { ConstantsSystem.StagingNameWaitingApproval, "#FFC220" },
        { ConstantsSystem.StagingNameApprove, "#59AD24" },
        { ConstantsSystem.StagingNameReject, "#FF0000" },
        { ConstantsSystem.StagingNameClose, "#FF0000" },
    };
}
