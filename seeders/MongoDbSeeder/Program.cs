using Catalog.API.Extensions;
using Catalog.API.Mapping;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.Mapping;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Logging.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MongoDbSeeder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // 1. Keep your Seeder specific setups at the very top
            MongoDbMapper.MapClasses();
            MapsterConfig.Configure(); // Pulled from your API setup

            // Mapster/TypeAdapter configs specific to your seeder mapping
            TypeAdapterConfig<BaseComponent, ComponentDto>.NewConfig()
                    .Map(dest => dest.Type, src => src.GetType().Name.ToLower())
                    .Map(dest => dest.Specifications, src => src.UniqueAttributesToMetadata());

            // 2. Set up the builder identical to your API
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton(new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { JsonMappingHelper.ConfigureJsonPolymorphism }
                }
            });

            // --- PASTE YOUR API SERVICES SETUP HERE ---
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddMongoDbServices(builder.Configuration);
            builder.Services.AddMassTransitServices(); // MassTransit will auto-create collections if used

            builder.Services.AddHttpClient("CatalogClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7022/catalog/");
            });

            builder.Host.AddObservability();
            builder.Services.AddObservability(builder.Configuration, "Catalog.MongoDbSeeder"); // Hardcoded name for seeder context

            // 3. Register your Seeder classes into this container
            builder.Services.AddScoped<MasterSeeder>();
            builder.Services.AddScoped<ISubSeeder, ComponentsSeeder>();
            builder.Services.AddScoped<ISubSeeder, ProductsSeeder>();

            // 4. Build the application instance
            var app = builder.Build();

            // 5. INSTEAD OF app.Run(), execute your seeding logic
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<MasterSeeder>();
                Console.WriteLine("Starting Database Seeding...");
                await seeder.SeedAsync();
                Console.WriteLine("Database Seeding Completed Successfully!");
            }

            // App closes nicely here instead of hanging forever listening to HTTP requests
        }
    }
}
