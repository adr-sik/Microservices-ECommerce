using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Products
{
    public record CreateLaptopRequest(
        string Brand,
        string Model,
        decimal Price,
        string? Description,
        string CpuId,
        string GpuId,
        string DisplayId
    ) : CreateProductRequest(ProductType.Laptop, Brand, Model, Price, Description);
}
