
using Catalog.API.Extensions;
using Catalog.API.Mapping;
using Catalog.API.Middleware;
using Catalog.API.Services;
using Catalog.Application.Mapping;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Shared.Logging.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddMongoDbServices(builder.Configuration);
            builder.Services.AddMassTransitServices();

            builder.Host.AddObservability();
            builder.Services.AddObservability(builder.Configuration, builder.Environment.ApplicationName);

            // 1. Define the shared configuration
            Action<JsonSerializerOptions> configureJson = options =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.PropertyNamingPolicy = null;
                options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { JsonMappingHelper.ConfigureJsonPolymorphism }
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
            };

            // 2. Apply to Global/Minimal API/OpenAPI
            builder.Services.Configure<JsonOptions>(opt => configureJson(opt.SerializerOptions));

            // 3. Apply to Controllers
            builder.Services.AddControllers().AddJsonOptions(opt => configureJson(opt.JsonSerializerOptions));

            //builder.Services.AddOpenApi();
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Servers = new List<OpenApiServer>
                        {
                            new() { Url = "https://localhost:7022/catalog" }
                        };
                    return Task.CompletedTask;
                });
            });

            builder.Services.AddForwardedHandler();

            builder.Services.AddGrpc();

            MapsterConfig.Configure();

            var app = builder.Build();

            app.MapGrpcService<GrpcProductsService>();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            app.UseForwardedHeaders();
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
