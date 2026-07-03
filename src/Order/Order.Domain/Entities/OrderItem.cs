namespace Ordering.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; init; }
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        // TODO: Change to a ValueObject
        public decimal Price { get; private set; }
        public decimal Subtotal => Price * Quantity;

        private OrderItem() { }
        public static OrderItem Create(
            string productId,
            string productName,
            int quantity,
            decimal price)
        {

            // TODO: Validation

            return new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };
        }


    }
}
