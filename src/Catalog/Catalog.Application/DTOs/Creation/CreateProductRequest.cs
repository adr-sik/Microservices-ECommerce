using Catalog.Application.DTOs.ReadOnly;
using Catalog.Domain.Enums;
using MediatR;

namespace Catalog.Application.DTOs.Creation
{
    public abstract record CreateProductRequest(
        ProductType Type,
        string Brand,
        string Model,
        decimal Price,
        string? Description
        ) : IRequest<ProductDto>;
}
