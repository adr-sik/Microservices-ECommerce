namespace Catalog.Application.Objects
{
    public record CreateProductRequest(string Type, string Brand, string Model, decimal Price, int Stock, string? Description, Dictionary<string, string>? Components);
}
