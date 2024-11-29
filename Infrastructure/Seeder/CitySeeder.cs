using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeders
{
    public class CitySeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CitySeeder> _logger;

        public CitySeeder(ApplicationDbContext context, ILogger<CitySeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (!_context.Cities.Any())
            {
                _logger.LogInformation("Seeding cities...");

                var cities = new[]
                {
                    new City { Name = "Jakarta" },
                    new City { Name = "Bandung" },
                    new City { Name = "Jogjakarta" },
                    new City { Name = "Semarang" },
                    new City { Name = "Surabaya" },
                    new City { Name = "Malang" }
                };

                _context.Cities.AddRange(cities);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cities seeded successfully.");
            }
            else
            {
                _logger.LogInformation("Cities already exist. Skipping seed.");
            }
        }
    }
}
