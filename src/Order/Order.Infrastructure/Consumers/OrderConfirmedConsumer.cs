using MassTransit;
using Ordering.Application.Interfaces;
using Ordering.Domain.Enums;
using Shared.Messages.Contracts.Saga.Events;

namespace Ordering.Infrastructure.Consumers
{
    public class OrderConfirmedConsumer(IOrdersService ordersService) : IConsumer<OrderConfirmed>
    {
        public async Task Consume(ConsumeContext<OrderConfirmed> context)
        {
            await ordersService.UpdateStatusAsync(context.Message.OrderId, OrderStatus.Confirmed, context.CancellationToken);
        }
    }
}
