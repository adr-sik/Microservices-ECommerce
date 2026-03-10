using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Filtering.Components
{
    public record CameraFilter : ComponentFilter
    {
        public ProductType? DesignedFor { get; set; }
        public int? MinMegapixels { get; set; }
        public int? MaxMegapixels { get; set; }
    }
}
