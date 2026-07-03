namespace Shared.Messages.Contracts.Saga.Events
{
    public record InventoryReservationFailed
    {
        public Guid OrderId { get; init; }
        public string Reason { get; init; }
    }
}
