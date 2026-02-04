using Catalog.Domain.Models.Enums;
using Catalog.Domain.Models.ProductComponents;

namespace Catalog.Domain.Models.ProductTypes
{
    public class Phone : Product
    {
        public required Cpu<MobileCpuBrand> Cpu { get; set; }
        public required Gpu<MobileGpuBrand> Gpu { get; set; }
        public required Display Display { get; set; }
        public required Camera Camera { get; set; }
    }
}
