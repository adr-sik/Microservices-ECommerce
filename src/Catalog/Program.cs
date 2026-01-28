
using Catalog.Application.Factories;
using Catalog.Application.Interfaces;
using Catalog.Application.Strategies;
using Catalog.Domain.Models;
using Catalog.Domain.Models.Enums;
using Catalog.Domain.Models.ProductComponents;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Services;
using Catalog.Presentation.Mapping;
using MongoDB.Bson.Serialization;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<CatalogDatabaseSettings>
                (builder.Configuration.GetSection("CatalogDatabase"));

            // Map MongoDB classes
            MongoDbMapper.MapClasses();

            builder.Services.AddSingleton<ProductsService>();
            builder.Services.AddSingleton<IComponentsService, ComponentsService>();
            builder.Services.AddScoped<ProductFactory>();

            builder.Services.AddScoped<ICreateProductStrategy, CreateLaptopStrategy>();

            BsonClassMap.RegisterClassMap<Cpu<ComputerCpuBrand>>();
            BsonClassMap.RegisterClassMap<Cpu<MobileCpuBrand>>();
            BsonClassMap.RegisterClassMap<Gpu<ComputerGpuBrand>>();
            BsonClassMap.RegisterClassMap<Display>();

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase,
                        allowIntegerValues: false));
                    options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                    {
                        Modifiers =
                        {
                            JsonMappingExtensions.ConfigureJsonPolymorphism
                        }
                    };
                });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
