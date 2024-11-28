// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsSystem
/// </summary>
public abstract class ConstantsSystem
{
    /// <summary>
    /// SystemName
    /// </summary>
    public const string Name = "System";

    /// <summary>
    /// SystemClientId
    /// </summary>
    public const string ClientId = "netca";

    /// <summary>
    /// SignatureType
    /// </summary>
    public const string SignatureType = "signature";

    /// <summary>
    /// EvidenceType
    /// </summary>
    public const string EvidenceType = "evidence";

    /// <summary>
    /// StagingCreate
    /// </summary>
    public const int StagingCreate = 1;

    /// <summary>
    /// StagingAssign
    /// </summary>
    public const int StagingAssign = 2;

    /// <summary>
    /// StagingWaitingApproval
    /// </summary>
    public const int StagingWaitingApproval = 3;

    /// <summary>
    /// StagingApprove
    /// </summary>
    public const int StagingApprove = 4;

    /// <summary>
    /// StagingReject
    /// </summary>
    public const int StagingReject = 99;

    /// <summary>
    /// StagingClose
    /// </summary>
    public const int StagingClose = 0;

    /// <summary>
    /// StagingNameCreate
    /// </summary>
    public const string StagingNameCreate = "Created";

    /// <summary>
    /// StagingNameAssign
    /// </summary>
    public const string StagingNameAssign = "Assigned";

    /// <summary>
    /// StagingNameWaitingApproval
    /// </summary>
    public const string StagingNameWaitingApproval = "Waiting Approval";

    /// <summary>
    /// StagingNameApprove
    /// </summary>
    public const string StagingNameApprove = "Approved";

    /// <summary>
    /// StagingNameReject
    /// </summary>
    public const string StagingNameReject = "Rejected";

    /// <summary>
    /// StagingNameClose
    /// </summary>
    public const string StagingNameClose = "Closed";

    /// <summary>
    /// StatusStagingByUser
    /// </summary>
    public const int StatusStagingByUser = 1;

    /// <summary>
    /// StatusStagingByApproval
    /// </summary>
    public const int StatusStagingBySpv = 2;

    /// <summary>
    /// StatusStagingByApproval
    /// </summary>
    public const int StatusStagingByApproval = 3;

    /// <summary>
    /// StatusStagingByReject
    /// </summary>
    public const int StatusStagingByReject = 4;

    #region email configuration

    /// <summary>
    /// TemplateApproval
    /// </summary>
    public const string TemplateApproval = "TemplateApproval";

    /// <summary>
    /// TemplateAssigned
    /// </summary>
    public const string TemplateAssigned = "TemplateAssigned";

    /// <summary>
    /// TemplateRejected
    /// </summary>
    public const string TemplateRejected = "TemplateRejected";

    /// <summary>
    /// UrlApproval
    /// </summary>
    public const string UrlApproval = "UrlApproval";

    /// <summary>
    /// HeaderEmail
    /// </summary>
    public const string HeaderEmail = "General Affair System";

    #endregion
}
