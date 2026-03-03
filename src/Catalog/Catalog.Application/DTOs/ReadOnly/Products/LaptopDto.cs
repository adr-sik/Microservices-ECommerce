namespace Catalog.Application.DTOs.ReadOnly.Products
{
    public record LaptopDto(
        string Id,
        string Brand,
        string Model,
        decimal Price,
        string? Description,
        ComponentDto Cpu,
        ComponentDto Gpu,
        ComponentDto Display
    ) : ProductDto(Id, Brand, Model, Price, Description);
}
