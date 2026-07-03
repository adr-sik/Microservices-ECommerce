namespace Catalog.Application.DTOs.Pagination
{
    public record PaginatedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
    {
        public int TotalPages => TotalCount == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
