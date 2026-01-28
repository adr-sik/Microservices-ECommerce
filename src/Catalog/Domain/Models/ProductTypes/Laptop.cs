using Catalog.Domain.Models.Enums;
using Catalog.Domain.Models.ProductComponents;

namespace Catalog.Domain.Models.ProductTypes
{
    public class Laptop : Product
    {
        public Cpu<ComputerCpuBrand>? Cpu { get; set; }
        public Gpu<ComputerGpuBrand>? Gpu { get; set; }
        public Display? Display { get; set; }
    }
}
