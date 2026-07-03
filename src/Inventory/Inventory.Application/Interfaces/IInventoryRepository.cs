using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<InventoryItem> CreateAsync(string productId);
        Task<InventoryItem?> GetAsync(Guid id);
        Task<List<InventoryItem>> GetAsync();
        Task UpdateAsync(InventoryItem item);
        Task DeleteAsync(Guid id);
        Task<List<InventoryItem>> GetByProductIdsAsync(List<string> productIds, CancellationToken ct = default);
        Task UpdateManyAsync(List<InventoryItem> items, CancellationToken ct = default);
    }
}
