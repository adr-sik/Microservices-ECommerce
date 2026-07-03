using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Messages.Contracts.Saga;

namespace Inventory.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryRepository inventoryRepository, ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        public async Task<InventoryItem> CreateAsync(string productId)
        {
            return await _inventoryRepository.CreateAsync(productId);
        }

        public async Task<bool> TryReserveAsync(List<OrderItemContract> items, CancellationToken ct)
        {
            var productIds = items.Select(i => i.ProductId).ToList();
            var inventoryItems = await _inventoryRepository.GetByProductIdsAsync(productIds, ct);

            _logger.LogInformation("Found {Count} inventory items for {ProductCount} products",
                inventoryItems.Count, productIds.Count);

            foreach (var item in items)
            {
                var inventoryItem = inventoryItems.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (inventoryItem == null || inventoryItem.Available < item.Quantity)
                {
                    _logger.LogWarning("Reservation failed for product {ProductId}", item.ProductId);
                    return false;
                }
            }

            foreach (var item in items)
            {
                var inventoryItem = inventoryItems.First(i => i.ProductId == item.ProductId);
                inventoryItem.ReserveStock(item.Quantity);
                _logger.LogInformation("Reserved {Quantity} for product {ProductId}, new Reserved={Reserved}",
                    item.Quantity, item.ProductId, inventoryItem.Reserved);
            }

            await _inventoryRepository.UpdateManyAsync(inventoryItems, ct);
            _logger.LogInformation("UpdateManyAsync completed");
            return true;
        }
    }
}
