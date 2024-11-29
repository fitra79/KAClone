// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Domain.Entities;
using Application.Common.Interfaces;
using Infrastructure.Persistence.Interceptors;

#endregion

namespace Infrastructure.Persistence;

/// <summary>
/// ApplicationDbContext
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
    : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Person> Persons { get; set; }

    public DbSet<ToDoList> ToDoLists { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public DbSet<City> Cities { get; set; }

    public void AsNoTracking()
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public void Clear()
    {
        ChangeTracker.Clear();
    }

    public async Task ExecuteResiliencyAsync(Func<Task> action)
    {
        var strategy = Database.CreateExecutionStrategy();
        await strategy
            .ExecuteAsync(async () =>
            {
                var transaction = await Database.BeginTransactionAsync().ConfigureAwait(false);
                await using (transaction.ConfigureAwait(false))
                {
                    await action().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            })
            .ConfigureAwait(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}


