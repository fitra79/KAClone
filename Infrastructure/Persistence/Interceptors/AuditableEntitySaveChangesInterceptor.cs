// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.Common.Interfaces;
using Domain.Common;

namespace Infrastructure.Persistence.Interceptors;

/// <summary>
/// AuditableEntitySaveChangesInterceptor
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeOffset _dateTimeOffset;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntitySaveChangesInterceptor"/> class.
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    public AuditableEntitySaveChangesInterceptor(
        IDateTimeOffset dateTimeOffset)
    {
        _dateTimeOffset = dateTimeOffset;
    }

    /// <summary>
    /// SavingChanges
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// SavingChangesAsync
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// UpdateEntities
    /// </summary>
    /// <param name="context"></param>
    private void UpdateEntities(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            var entity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
                    entity.CreatedDate ??= _dateTimeOffset.Now;
                    entity.IsActive ??= true;
                    entity.IsDelete ??= false;
                    break;
                case EntityState.Modified:
                    entity.UpdatedDate = _dateTimeOffset.Now;
                    break;
            }

        }
    }
}

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// HasChangedOwnedEntities
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry
            .References
            .Any(
                r =>
                    r.TargetEntry != null
                    && r.TargetEntry.Metadata.IsOwned()
                    && r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
