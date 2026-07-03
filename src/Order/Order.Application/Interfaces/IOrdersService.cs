using Ordering.Application.DTOs;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.Interfaces
{
    public interface IOrdersService
    {
        Task<Order> CreateAsync(CreateOrderRequest request, CancellationToken ct);
        Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus, CancellationToken ct);
    }
}
