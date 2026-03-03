using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Filtering.Components
{
    public record CpuFilter : ComponentFilter
    {
        public ProductType? DesignedFor { get; set; }
        public CpuBrand? Brand { get; set; }
        public int? MinNumberOfCores { get; set; }
        public int? MaxNumberOfCores { get; set; }
    }
}
