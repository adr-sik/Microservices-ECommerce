using Inventory.Application.Interfaces;
using Inventory.Application.Services;
using Inventory.Infrastructure.Consumers;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using MassTransit;
using Scalar.AspNetCore;
using Shared.Logging.Extensions;

namespace Inventory.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.AddObservability();
            builder.Services.AddObservability(builder.Configuration, builder.Environment.ApplicationName);
            builder.Services.AddInfrastructureDI(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Application
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            // Infrastructure
            builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CatalogItemCreatedConsumer>();
                x.AddConsumer<ReserveStockConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost");
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();

                app.MapGet("/", () => Results.Redirect("/scalar/v1"))
                    .ExcludeFromDescription();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
