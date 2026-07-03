namespace Shared.Messages.Contracts.Saga.Events
{
    public record InventoryReserved
    {
        public Guid OrderId { get; init; }
    }
}
