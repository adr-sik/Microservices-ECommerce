using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrderDbContext _context;

        public OrdersRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order?> GetAsync(Guid id)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
        {
            await _context.Orders.AddAsync(order, ct);
            return order;
        }
        public async Task UpdateAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus, CancellationToken ct)
        {
            await _context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, newStatus), ct);
        }
    }
}
