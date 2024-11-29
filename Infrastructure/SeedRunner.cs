using Infrastructure.Persistence.Seeders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.SeederRunner
{
    public class SeederRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeederRunner> _logger;

        public SeederRunner(IServiceProvider serviceProvider, ILogger<SeederRunner> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var citySeeder = services.GetRequiredService<CitySeeder>();

            _logger.LogInformation("Starting seeder...");
            await citySeeder.SeedAsync();
            _logger.LogInformation("Seeder completed.");
        }
    }
}
