namespace Ordering.Application.DTOs
{
    public record OrderItemDto(
        string ProductId,
        string ProductName,
        int Quantity,
        decimal PricePerUnit
        );
}
