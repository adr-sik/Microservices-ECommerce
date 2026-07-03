namespace Shared.Messages.Contracts.Saga.Events
{
    public record OrderCancelled
    {
        public Guid OrderId { get; init; }
        public string Reason { get; init; }
    }
}
