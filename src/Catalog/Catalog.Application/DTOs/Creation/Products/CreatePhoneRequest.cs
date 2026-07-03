using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Products
{
    public record CreatePhoneRequest(
        string Brand,
        string Model,
        decimal Price,
        string? Description,
        string CpuId,
        string GpuId,
        string DisplayId,
        string CameraId
    ) : CreateProductRequest(ProductType.Phone, Brand, Model, Price, Description);
}
