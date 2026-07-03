using MassTransit;
using Shared.Messages.Contracts.Saga.Events;
using System.Text.Json;

namespace Ordering.Infrastructure.Saga
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State ReservingInventory { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }


        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<InventoryReserved> InventoryReserved { get; private set; }
        public Event<InventoryReservationFailed> InventoryReservationFailed { get; private set; }
        //public Event<OrderCancelled> OrderFailed { get; private set; }
        public Event<OrderConfirmed> OrderConfirmed { get; private set; }

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => InventoryReserved, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => InventoryReservationFailed, x => x.CorrelateById(m => m.Message.OrderId));
            //Event(() => OrderFailed, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderConfirmed, x => x.CorrelateById(m => m.Message.OrderId));

            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Saga.OrderDate = DateTime.UtcNow;
                        context.Saga.SerializedItems = JsonSerializer.Serialize(context.Message.Items);
                        context.Saga.RetryCount = 0;
                    })
                    .PublishAsync(context => context.Init<ReserveInventory>(new
                    {
                        OrderId = context.Saga.CorrelationId,
                        Items = context.Message.Items
                    }))
                    .TransitionTo(ReservingInventory)
                );

            During(ReservingInventory,
                When(InventoryReserved)
                    .PublishAsync(context => context.Init<OrderConfirmed>(new
                    {
                        OrderId = context.Saga.CorrelationId
                    }))
                    .TransitionTo(Completed)
                    .Finalize(),
                When(InventoryReservationFailed)
                    .PublishAsync(context => context.Init<OrderCancelled>(new
                    {
                        OrderId = context.Saga.CorrelationId
                    }))
                    .TransitionTo(Failed)
                    .Finalize()
                );

            SetCompletedWhenFinalized();
        }
    }
}
