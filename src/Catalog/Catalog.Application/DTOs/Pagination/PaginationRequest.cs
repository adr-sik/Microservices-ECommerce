namespace Catalog.Application.DTOs.Pagination
{
    public record PaginationRequest
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int Skip => (Page - 1) * PageSize;
        public int Take => PageSize;

        public PaginationRequest(int page = 1, int pageSize = 10)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize switch
            {
                < 10 => 10,
                > 50 => 50,
                _ => pageSize
            };
        }
    }
}
