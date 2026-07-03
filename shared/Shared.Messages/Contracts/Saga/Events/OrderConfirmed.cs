namespace Shared.Messages.Contracts.Saga.Events
{
    public record OrderConfirmed
    {
        public Guid OrderId { get; init; }
    }
}
