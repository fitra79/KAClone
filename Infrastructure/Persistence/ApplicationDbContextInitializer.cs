// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

/// <summary>
/// ApplicationDbContextInitializer
/// </summary>
public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContextInitializer"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// InitializeAsync
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                _logger.LogWarning("Migrating Database");
                await _context.Database.MigrateAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }

    /// <summary>
    /// SeedAsync
    /// </summary>
    /// <returns></returns>
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    /// <summary>
    /// TrySeedAsync
    /// </summary>
    private static async Task TrySeedAsync()
    {
        await Task.Delay(0).ConfigureAwait(false);
    }
}
