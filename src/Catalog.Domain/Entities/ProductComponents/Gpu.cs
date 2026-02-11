using Catalog.Domain.Constraints;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductComponents
{
    public class Gpu : BaseComponent, IDesignConstraint
    {
        public required Enums.ProductTypes DesignedFor { get; set; }
        public required GpuBrand Brand { get; set; }
        public required string Model { get; set; }
        public int VRAM { get; set; }
    }
}
