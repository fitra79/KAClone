// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// DepartmentConfiguration
/// </summary>
public class PersonConfiguration : AuditTableConfiguration<Person>
{
    /// <summary>
    /// Configure Department
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnType("varchar(20)")
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasColumnType("varchar(255)")
            .HasMaxLength(255);

        base.Configure(builder);
    }
}
