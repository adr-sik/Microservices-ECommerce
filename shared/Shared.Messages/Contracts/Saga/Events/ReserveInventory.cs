namespace Shared.Messages.Contracts.Saga.Events
{
    public record ReserveInventory
    {
        public Guid OrderId { get; init; }
        public List<OrderItemContract> Items { get; init; }
    }
}
