using Inventory.Domain.Entities;
using Shared.Messages.Contracts.Saga;

namespace Inventory.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryItem> CreateAsync(string productId);
        Task<bool> TryReserveAsync(List<OrderItemContract> items, CancellationToken ct);
    }
}
