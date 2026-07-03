namespace Shared.Messages.Contracts.Messages
{
    public record CatalogItemCreated
    {
        public required string ItemId { get; init; }
        public DateTime CreatedAt { get; init; }
    };
}
