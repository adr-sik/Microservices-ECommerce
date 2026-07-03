using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.Interfaces
{
    public interface IOrdersRepository
    {
        Task<Order> CreateAsync(Order order, CancellationToken ct = default);
        Task<Order?> GetAsync(Guid id);
        Task<List<Order>> GetAsync();
        Task UpdateAsync(Order order, CancellationToken ct = default);
        Task DeleteAsync(Guid id);
        Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus, CancellationToken ct);
    }
}
