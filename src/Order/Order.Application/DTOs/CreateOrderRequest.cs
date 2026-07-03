namespace Ordering.Application.DTOs
{
    public record CreateOrderRequest(
        string UserId,
        List<CartItem> Cart
        );
}
