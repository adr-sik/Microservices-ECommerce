using Catalog.Application.DTOs.ReadOnly;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoDbSeeder
{
    public class MasterSeeder
    {
        private readonly MongoClient _mongoClient;
        private readonly IEnumerable<ISubSeeder> _subSeeders;
        public List<BaseComponent> generatedComponents { get; set; } = new();

        public MasterSeeder(
            IOptions<CatalogDatabaseSettings> catalogDatabaseSettings,
            IEnumerable<ISubSeeder> subSeeders)
        {
            _mongoClient = new MongoClient(catalogDatabaseSettings.Value.ConnectionString);
            _subSeeders = subSeeders;
        }

        public async Task SeedAsync()
        {
            Console.WriteLine("Starting database seeding...");
            try
            {
                Console.WriteLine("Clearing existing data...");
                await _mongoClient.DropDatabaseAsync("CatalogDb");
                Console.WriteLine("Existing data cleared.");

                foreach (var subSeeder in _subSeeders)
                {
                    Console.WriteLine($"Working - {subSeeder.GetType().Name}...");
                    await subSeeder.SeedAsync(generatedComponents);
                }

                Console.WriteLine("Database seeded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CRITICAL ERROR DURING SEEDING]: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
