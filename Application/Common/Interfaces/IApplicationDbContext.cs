// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#region

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#endregion

namespace Application.Common.Interfaces;

/// <summary>
/// IApplicationDbContext
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets or sets Directorates
    /// </summary>
    DbSet<Person> Persons { get; set; }

    DbSet<ToDoList> ToDoLists { get; set; }

    DbSet<ToDoItem> ToDoItems { get; set; }

    /// <summary>
    /// Gets database
    /// </summary>
    DatabaseFacade Database { get; }

    /// <summary>
    /// AsNoTracking
    /// </summary>
    void AsNoTracking();

    /// <summary>
    /// Clear
    /// </summary>
    void Clear();

    /// <summary>
    /// Execute using EF Core resiliency strategy
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    Task ExecuteResiliencyAsync(Func<Task> action);

    /// <summary>
    /// SaveChangesAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
