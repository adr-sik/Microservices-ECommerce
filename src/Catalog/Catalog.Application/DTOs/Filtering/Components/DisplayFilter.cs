using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Filtering.Components
{
    public record DisplayFilter : ComponentFilter
    {
        public ProductType? DesignedFor { get; set; }
        public double? MinScreenSizeInches { get; set; }
        public double? MaxScreenSizeInches { get; set; }
        public string? Resolution { get; set; }
        public int? MinRefreshRateHz { get; set; }
        public int? MaxRefreshRateHz { get; set; }
    }
}
