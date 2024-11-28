// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Common;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// AuditTableConfiguration
/// </summary>
/// <typeparam name="TBase"></typeparam>
public abstract class AuditTableConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : BaseAuditableEntity
{
    /// <summary>
    /// Configure for all entities
    /// </summary>
    /// <param name="builder"></param>
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasQueryFilter(e => e.IsActive.Value);

        builder.HasKey(e => e.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.IsDelete)
            .HasDefaultValue(false);

        // builder.Property(e => e.CreatedBy)
        //     .HasColumnType("varchar(150)")
        //     .HasMaxLength(150);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValue(DateTimeOffset.Now);

        // builder.Property(e => e.UpdatedBy)
        //     .HasColumnType("varchar(150)")
        //     .HasMaxLength(150);

        builder.HasIndex(b => b.Id);
        builder.HasIndex(b => b.IsActive);
        builder.HasIndex(b => b.IsDelete);
    }
}
