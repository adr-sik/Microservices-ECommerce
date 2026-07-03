using Ordering.Application.Interfaces;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Transactions
{
    public class PostgresUnitOfWork(OrderDbContext context) : IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken ct = default)
            => context.SaveChangesAsync(ct);
    }
}
