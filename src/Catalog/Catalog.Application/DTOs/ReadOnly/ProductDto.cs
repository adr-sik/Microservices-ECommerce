namespace Catalog.Application.DTOs.ReadOnly
{
    public record ProductDto(
        string Id,
        string Type,
        string Brand,
        string Model,
        decimal Price,
        string? Description,
        IReadOnlyList<ComponentDto> Components
        );
}
