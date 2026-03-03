namespace Catalog.Application.DTOs.ReadOnly.Products
{
    public record PhoneDto(
        string Id,
        string Brand,
        string Model,
        decimal Price,
        string? Description,
        ComponentDto Cpu,
        ComponentDto Gpu,
        ComponentDto Display,
        ComponentDto Camera
    ) : ProductDto(Id, Brand, Model, Price, Description);
}
