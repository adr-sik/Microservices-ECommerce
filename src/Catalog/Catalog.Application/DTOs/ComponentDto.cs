namespace Catalog.Application.DTOs
{
    public record ComponentDto(
        string Id,
        Dictionary<string, object> Specifications
        );
}
