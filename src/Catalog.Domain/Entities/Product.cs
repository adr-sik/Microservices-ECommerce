namespace Catalog.Domain.Entities
{
    public abstract class Product
    {
        public string? Id { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
    }
}
