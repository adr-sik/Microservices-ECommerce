using Catalog.Application.Factories;
using Catalog.Application.Interfaces;
using Catalog.Application.Strategies;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Bson.Serialization;

namespace Catalog.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ProductsService>();
            services.AddSingleton<IComponentsService, ComponentsService>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ProductFactory>();
            services.AddScoped<ICreateProductStrategy, CreateLaptopStrategy>();

            return services;
        }

        public static IServiceCollection AddForwardedHandler(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto |
                    ForwardedHeaders.XForwardedHost;

                // TODO: Remove
                // Placeholder until deployed to cloud environment
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            return services;
        }

        public static IServiceCollection AddMongoDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CatalogDatabaseSettings>
                (configuration.GetSection("CatalogDatabase"));
            MongoDbMapper.MapClasses();

            BsonClassMap.RegisterClassMap<Cpu<ComputerCpuBrand>>();
            BsonClassMap.RegisterClassMap<Cpu<MobileCpuBrand>>();
            BsonClassMap.RegisterClassMap<Gpu<ComputerGpuBrand>>();
            BsonClassMap.RegisterClassMap<Display>();

            return services;
        }
    }
}
