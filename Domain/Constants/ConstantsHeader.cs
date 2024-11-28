// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsHeader
/// </summary>
public abstract class ConstantsHeader
{
    /// <summary>
    /// OcpApimSubscriptionKey
    /// </summary>
    public const string OcpApimSubscriptionKey = "Ocp-Apim-Subscription-Key";

    /// <summary>
    /// HeaderJson
    /// </summary>
    public const string Json = "application/json";

    /// <summary>
    /// HeaderJsonVndApi
    /// </summary>
    public const string JsonVndApi = "application/vnd.api+json";

    /// <summary>
    /// HeaderPdf
    /// </summary>
    public const string Pdf = "application/pdf";

    /// <summary>
    /// HeaderTextPlain
    /// </summary>
    public const string TextPlain = "text/plain";

    /// <summary>
    /// HeaderOctetStream
    /// </summary>
    public const string OctetStream = "application/octet-stream";

    /// <summary>
    /// HeaderProblemJson
    /// </summary>
    public const string ProblemJson = "application/problem+json";

    /// <summary>
    /// HeaderTextCsv
    /// </summary>
    public const string TextCsv = "text/csv";

    /// <summary>
    /// HeaderExcelXls
    /// </summary>
    public const string ExcelXls = "application/vnd.ms-excel";

    /// <summary>
    /// HeaderExcelXlsx
    /// </summary>
    public const string ExcelXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// HeaderImageJpg
    /// </summary>
    public const string ImageJpg = "image/jpg";

    /// <summary>
    /// HeaderMultipart
    /// </summary>
    public const string Multipart = "multipart/form-data";

    /// <summary>
    /// HeaderZip
    /// </summary>
    public const string Zip = "application/zip";

    /// <summary>
    /// HeaderIfNoneMatch
    /// </summary>
    public const string IfNoneMatch = "If-None-Match";

    /// <summary>
    /// HeaderETag
    /// </summary>
    public const string ETag = "ETag";
}
