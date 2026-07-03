using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("Database");
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
