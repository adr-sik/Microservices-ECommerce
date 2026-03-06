namespace Catalog.Application.DTOs.ReadOnly
{
    public record ComponentDto(
        string Id,
        string Type,
        Dictionary<string, object> Specifications
        );
}
