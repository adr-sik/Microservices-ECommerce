namespace Catalog.Application.DTOs
{
    public abstract record ProductDto(
        string Id,
        string Brand,
        string Model,
        decimal Price,
        string? Description
        );
}
