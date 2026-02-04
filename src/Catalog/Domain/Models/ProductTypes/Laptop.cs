using Catalog.Domain.Models.Enums;
using Catalog.Domain.Models.ProductComponents;

namespace Catalog.Domain.Models.ProductTypes
{
    public class Laptop : Product
    {
        public required Cpu<ComputerCpuBrand> Cpu { get; set; }
        public required Gpu<ComputerGpuBrand> Gpu { get; set; }
        public required Display Display { get; set; }
    }
}
