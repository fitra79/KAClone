// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#region

using System;

#endregion

namespace Domain.Common;

/// <summary>
/// BaseAuditableEntity
/// </summary>
public abstract record BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets createdDate
    /// </summary>
    /// <value></value>
    public DateTimeOffset? CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets updatedDate
    /// </summary>
    /// <value></value>
    public DateTimeOffset? UpdatedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether isActive
    /// </summary>
    /// <value></value>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indication whether isDelete
    /// </summary>
    /// <value></value>
    public bool? IsDelete { get; set; }
}
