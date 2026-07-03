using Inventory.Application.Interfaces;
using MassTransit;
using Shared.Messages.Contracts.Saga.Events;

namespace Inventory.Infrastructure.Consumers
{
    public class ReleaseStockConsumer(IInventoryService inventoryService) : IConsumer<OrderCancelled>
    {
        public Task Consume(ConsumeContext<OrderCancelled> context)
        {
            throw new NotImplementedException();
        }
    }
}
