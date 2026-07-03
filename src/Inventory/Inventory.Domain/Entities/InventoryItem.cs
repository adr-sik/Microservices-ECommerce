namespace Inventory.Domain.Entities
{
    public class InventoryItem
    {
        public Guid Id { get; init; }
        public string ProductId { get; set; }
        public int Stock { get; private set; }
        public int Reserved { get; private set; }
        public int Version { get; set; }

        public int Available => Stock - Reserved;

        private InventoryItem() { }

        public static InventoryItem Create(string productId)
        {
            return new InventoryItem
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Stock = 0,
                Reserved = 0
            };
        }

        public void AddStock(int amount)
        {
            Stock += amount;
        }

        public void ReserveStock(int quantity)
        {
            if (quantity > Available)
                throw new InvalidOperationException("Insufficient stock.");
            Reserved += quantity;
        }

        public void ReleaseStock(int quantity)
        {
            Reserved -= quantity;
        }
    }
}
