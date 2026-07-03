using Inventory.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages.Contracts.Saga.Events;

namespace Inventory.Infrastructure.Consumers
{
    public class ReserveStockConsumer(IInventoryService inventoryService, ILogger<ReserveStockConsumer> logger) : IConsumer<ReserveInventory>
    {
        public async Task Consume(ConsumeContext<ReserveInventory> context)
        {
            try
            {
                var success = await inventoryService.TryReserveAsync(context.Message.Items, context.CancellationToken);

                if (!success)
                {
                    await context.Publish<InventoryReservationFailed>(new
                    {
                        OrderId = context.Message.OrderId
                    });
                    return;
                }

                await context.Publish<InventoryReserved>(new
                {
                    OrderId = context.Message.OrderId
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reserve inventory for order {OrderId}", context.Message.OrderId);

                await context.Publish<InventoryReservationFailed>(new
                {
                    OrderId = context.Message.OrderId
                });

            }
        }
    }
}
