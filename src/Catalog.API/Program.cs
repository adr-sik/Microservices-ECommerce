
using Catalog.API.Extensions;
using Catalog.API.Mapping;
using Scalar.AspNetCore;
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
                            JsonMappingHelper.ConfigureJsonPolymorphism
                        }
                    };
                });
            builder.Services.AddOpenApi();

            builder.Services.AddForwardedHandler();

            var app = builder.Build();

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
