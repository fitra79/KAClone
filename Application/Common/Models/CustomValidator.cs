// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.AspNetCore.Http;
using Domain.Constants;

namespace Application.Common.Models;

/// <summary>
/// CustomValidator
/// </summary>
public static class CustomValidator
{
    internal static bool IsValidGuid(string unValidatedGuid, bool ignoreNull = false)
    {
        try
        {
            var result = !string.IsNullOrEmpty(unValidatedGuid) && Guid.TryParse(unValidatedGuid, out _);

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    internal static bool MustNullString(string arg)
    {
        return arg is not null;
    }

    internal static bool MustNullDate(DateTime? arg)
    {
        return arg is not null;
    }

    internal static bool IsValidDate(DateTime? arg)
    {
        return !arg.Equals(default(DateTime));
    }

    internal static bool MaximumLengthBase64(string arg, int maxLength = ConstantsValidation.MaximumLengthBase64)
    {
        var length = Encoding.UTF8.GetByteCount(arg);

        return length <= maxLength;
    }

    internal static bool MaximumFileSize(long size, int maxLength = ConstantsValidation.MaximumFileSize)
    {
        return size <= maxLength;
    }

    internal static bool MaximumFileSizeSignature(long size, int maxLength = ConstantsValidation.MaximumFileSizeSignature)
    {
        return size <= maxLength;
    }

    internal static bool IsValidFileName(string fileName, List<string> fileNameList)
    {
        return fileNameList.Contains(fileName);
    }

    internal static bool IsValidFileType(string fileType, List<string> fileTypeList)
    {
        return fileTypeList.Contains(fileType);
    }

    internal static string PickColor(string arg)
    {
        return ConstantsProcess.ColorStagings[arg];
    }

    internal static bool IsValidContentTypeUploadMasterData(IFormFile file)
    {
        var validContentTypes = new[]
        {
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.ms-excel"
        };

        return validContentTypes.Contains(file.ContentType);
    }
}
