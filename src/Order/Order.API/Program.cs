
using Catalog.API.Protos;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ordering.API.Grpc;
using Ordering.Application.Interfaces;
using Ordering.Application.Services;
using Ordering.Infrastructure.Consumers;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Saga;
using Ordering.Infrastructure.Transactions;
using Scalar.AspNetCore;
using Shared.Logging.Extensions;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddInfrastructureDI(builder.Configuration);

            builder.Host.AddObservability();
            builder.Services.AddObservability(builder.Configuration, builder.Environment.ApplicationName);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Servers = new List<OpenApiServer>
                        {
                            new() { Url = "https://localhost:7022/ordering" }
                        };
                    return Task.CompletedTask;
                });
            });

            builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(options =>
            {
                options.Address = new Uri("https://localhost:7020");
            });

            builder.Services.AddScoped<ICatalogService, CatalogServiceAdapter>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
            builder.Services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();

            // Saga
            builder.Services.AddDbContext<OrderSagaDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                        r.AddDbContext<DbContext, OrderSagaDbContext>();
                        r.UsePostgres();
                    });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost");
                    cfg.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromMilliseconds(200),
                        TimeSpan.FromMilliseconds(500)));
                    cfg.ConfigureEndpoints(context);
                });

                x.AddConsumer<OrderConfirmedConsumer>();
                x.AddConsumer<OrderCancelledConsumer>();
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
