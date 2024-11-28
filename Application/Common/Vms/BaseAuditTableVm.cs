// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Vms;

/// <summary>
/// BaseAuditTableVm
/// </summary>
public class BaseAuditTableVm
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets createdBy
    /// </summary>
    /// <value></value>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets createdDate
    /// </summary>
    /// <value></value>
    public DateTimeOffset? CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets updatedBy
    /// </summary>
    /// <value></value>
    public string UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets updatedDate
    /// </summary>
    /// <value></value>
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether IsActive
    /// </summary>
    public bool IsActive { get; set; }
}
