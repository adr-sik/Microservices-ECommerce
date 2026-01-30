using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductTypes
{
    public class Phone : Product
    {
        public required Cpu<MobileCpuBrand> Cpu { get; set; }
        public required Gpu<MobileGpuBrand> Gpu { get; set; }
        public required Display Display { get; set; }
        public required Camera Camera { get; set; }
    }
}
