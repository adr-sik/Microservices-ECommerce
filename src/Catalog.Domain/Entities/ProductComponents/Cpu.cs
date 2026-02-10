using Catalog.Domain.Constraints;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductComponents
{
    public class Cpu : BaseComponent, IDesignConstraint
    {
        public required ProductTypesEnum DesignedFor { get; set; }
        public required CpuBrandEnum Brand { get; set; }
        public required string Model { get; set; }
        public int NumberOfCores { get; set; }
    }
}
