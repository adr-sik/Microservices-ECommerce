using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Filtering.Components
{
    public record GpuFilter : ComponentFilter
    {
        public ProductType? DesignedFor { get; set; }
        public GpuBrand? Brand { get; set; }
        public string? Model { get; set; }
        public int? MaxVRAM { get; set; }
        public int? MinVRAM { get; set; }
    }
}
