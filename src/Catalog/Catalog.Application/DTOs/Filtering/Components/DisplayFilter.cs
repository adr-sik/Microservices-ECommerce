using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Filtering.Components
{
    public record DisplayFilter : ComponentFilter
    {
        public ProductType? DesignedFor { get; set; }
        public decimal? MinScreenSizeInches { get; set; }
        public decimal? MaxScreenSizeInches { get; set; }
        public string? Resolution { get; set; }
        public int? MinRefreshRateHz { get; set; }
        public int? MaxRefreshRateHz { get; set; }
    }
}
