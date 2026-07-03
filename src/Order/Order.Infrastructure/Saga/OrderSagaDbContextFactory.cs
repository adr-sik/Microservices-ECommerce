using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ordering.Infrastructure.Saga
{
    public class OrderSagaDbContextFactory : IDesignTimeDbContextFactory<OrderSagaDbContext>
    {
        public OrderSagaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<OrderSagaDbContext>()
                .UseNpgsql(configuration.GetConnectionString("Database"))
                .Options;
            return new OrderSagaDbContext(options);
        }
    }
}
