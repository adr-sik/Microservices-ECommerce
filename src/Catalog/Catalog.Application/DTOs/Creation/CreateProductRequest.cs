using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation
{
    public abstract record CreateProductRequest(
        ProductType Type,
        string Brand,
        string Model,
        decimal Price,
        int Stock,
        string? Description
        );
}
