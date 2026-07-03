using MassTransit;
using Ordering.Application.Interfaces;
using Ordering.Domain.Enums;
using Shared.Messages.Contracts.Saga.Events;

namespace Ordering.Infrastructure.Consumers
{
    public class OrderCancelledConsumer(IOrdersService ordersService) : IConsumer<OrderCancelled>
    {
        public async Task Consume(ConsumeContext<OrderCancelled> context)
        {
            await ordersService.UpdateStatusAsync(context.Message.OrderId, OrderStatus.Cancelled, context.CancellationToken);
        }
    }
}
