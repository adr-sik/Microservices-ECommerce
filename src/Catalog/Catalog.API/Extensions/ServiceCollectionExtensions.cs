using Catalog.Application.Factories;
using Catalog.Application.Interfaces;
using Catalog.Application.Services;
using Catalog.Application.Strategies.Components;
using Catalog.Application.Strategies.Products;
using Catalog.Infrastructure.Outbox;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Transactions;
using MassTransit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IComponentsService, ComponentsService>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IComponentsRepository, ComponentsRepository>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductFactory, ProductFactory>();
            services.AddScoped<ICreateProductStrategy, CreateLaptopStrategy>();
            services.AddScoped<ICreateProductStrategy, CreatePhoneStrategy>();

            services.AddScoped<IComponentFactory, ComponentFactory>();
            services.AddScoped<ICreateComponentStrategy, CreateCpuStrategy>();
            services.AddScoped<ICreateComponentStrategy, CreateGpuStrategy>();
            services.AddScoped<ICreateComponentStrategy, CreateDisplayStrategy>();
            services.AddScoped<ICreateComponentStrategy, CreateCameraStrategy>();

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

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            services.AddScoped<CatalogMongoDbContext>();

            MongoDbMapper.MapClasses();

            services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();
            services.AddScoped(sp => (ITransactionIdProvider<IClientSessionHandle?>)sp.GetRequiredService<IUnitOfWork>());
            services.AddSingleton<OutboxCollection>();
            services.AddScoped<IOutboxWriter, OutboxWriter>();
            services.AddHostedService<OutboxRelayWorker>();

            return services;
        }

        public static IServiceCollection AddMassTransitServices(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost");
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
