namespace Catalog.Application.Objects
{
    public class CreateProductRequest
    {
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string>? Components { get; set; }
    }
}
