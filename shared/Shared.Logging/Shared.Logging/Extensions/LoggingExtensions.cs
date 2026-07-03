using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.OpenTelemetry;

namespace Shared.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostBuilder AddObservability(this IHostBuilder host) =>
            host.UseSerilog((ctx, cfg) => cfg
                .ReadFrom.Configuration(ctx.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry(o =>
                {
                    o.Endpoint = ctx.Configuration["Otel:CollectorEndpoint"];
                    o.Protocol = OtlpProtocol.Grpc;
                    o.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = ctx.HostingEnvironment.ApplicationName
                    };
                }));

        public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration config, string appName) =>
            services.AddOpenTelemetry()
                .UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(config["Otel:CollectorEndpoint"]!))
                .WithTracing(tracing => tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("MassTransit"))
                .Services;
    }
}
