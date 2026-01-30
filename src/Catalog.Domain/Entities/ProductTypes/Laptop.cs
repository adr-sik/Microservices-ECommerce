using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductTypes
{
    public class Laptop : Product
    {
        public required Cpu<ComputerCpuBrand> Cpu { get; set; }
        public required Gpu<ComputerGpuBrand> Gpu { get; set; }
        public required Display Display { get; set; }
    }
}
