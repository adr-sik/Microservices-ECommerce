using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs
{
    public record OrderDto(
        Guid Id,
        string UserId,
        OrderStatus Status,
        DateTime CreatedAt,
        DateTime? CompletedAt,
        decimal TotalPrice,
        List<OrderItemDto> Items
        );
}
