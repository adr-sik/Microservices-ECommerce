using Ordering.Domain.Enums;

namespace Ordering.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; init; }
        // TODO: Fix this to match User entity
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal TotalPrice { get; set; }


        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Order() { }

        public static Order Create(string userId, List<OrderItem> items)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in items)
            {
                order._items.Add(item);
            }

            order.CalculateTotal();

            return order;
        }

        private void CalculateTotal()
        {
            TotalPrice = _items.Sum(item => item.Subtotal);
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
