namespace Shared.Messages.Contracts.Saga
{
    public record OrderItemContract
    {
        public string ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal Price { get; init; }
    }
}
