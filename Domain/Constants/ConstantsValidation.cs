// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsValidation
/// </summary>
public abstract class ConstantsValidation
{
    /// <summary>
    /// Maximum Length Base64 in byte
    /// </summary>
    public const int MaximumLengthBase64 = 512 * 1024;

    /// <summary>
    /// Maximum File Size in byte
    /// </summary>
    public const int MaximumFileSize = 4096 * 1024;

    /// <summary>
    /// Maximum File Signature Size in byte
    /// </summary>
    public const int MaximumFileSizeSignature = 512 * 1024;
}
