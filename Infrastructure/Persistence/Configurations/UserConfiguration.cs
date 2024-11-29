using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// UserConfiguration
/// </summary>
public class UserConfiguration : AuditTableConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(x => x.Nama)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);

        builder.Property(x => x.Ktp)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);

        builder.Property(x => x.Passport)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(x => x.Birthdate);

        builder.Property(x => x.Email)
            .HasColumnType("varchar(20)")
            .HasMaxLength(20);

        base.Configure(builder);
    }
}
