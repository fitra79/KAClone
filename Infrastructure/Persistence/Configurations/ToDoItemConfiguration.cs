// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// DepartmentConfiguration
/// </summary>
public class ToDoItemConfiguration : AuditTableConfiguration<ToDoItem>
{
    /// <summary>
    /// Configure Department
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.Property(x => x.IsCompleted)
            .HasDefaultValue(false);

        base.Configure(builder);
    }
}
