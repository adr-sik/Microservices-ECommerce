namespace Catalog.Application.DTOs.Sorting
{
    public record ProductSortRequest
    (
        ProductSortColumn Column,
        ProductSortDirection Direction
    );
}
