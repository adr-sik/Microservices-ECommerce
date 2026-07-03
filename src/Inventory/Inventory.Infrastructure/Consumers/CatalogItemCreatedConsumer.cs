using Inventory.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

using Shared.Messages.Contracts.Messages;

namespace Inventory.Infrastructure.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<CatalogItemCreatedConsumer> _logger;

        public CatalogItemCreatedConsumer(IInventoryService inventoryService, ILogger<CatalogItemCreatedConsumer> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            _logger.LogInformation("Received CatalogItemCreated for ProductId={ProductId}", context.Message.ItemId);
            await _inventoryService.CreateAsync(context.Message.ItemId);
            _logger.LogInformation("Inventory item created for ProductId={ProductId}", context.Message.ItemId);
        }
    }
}
