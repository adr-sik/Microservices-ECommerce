using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Products
{
    public record CreateLaptopRequest(
        ProductType Type,
        string Brand,
        string Model,
        decimal Price,
        int Stock, string?
        Description,

        string CpuId,
        string GpuId,
        string DisplayId
    ) : CreateProductRequest(Type, Brand, Model, Price, Stock, Description);
}
