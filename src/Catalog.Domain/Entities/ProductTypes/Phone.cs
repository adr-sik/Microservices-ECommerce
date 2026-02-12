using Catalog.Domain.Attributes;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;
using Catalog.Domain.Extensions;

namespace Catalog.Domain.Entities.ProductTypes
{
    public class Phone : Product
    {
        public override ProductType Type => ProductType.Phone;
        public Cpu Cpu { get; private set; }
        public Gpu Gpu { get; private set; }
        public Display Display { get; set; }
        public Camera Camera { get; set; }

        public Phone(
            string brand, string model, decimal price, int stock, string? description, Cpu cpu, Gpu gpu, Display display, Camera camera)
            : base(brand, model, price, stock, description)
        {
            SetCpu(cpu);
            SetGpu(gpu);
            Display = display;
            Camera = camera;
        }
        
        public void SetCpu(Cpu cpu)
        {
            this.ValidateComponent(cpu);
            Cpu = cpu;
        }

        public void SetGpu(Gpu gpu)
        {
            this.ValidateComponent(gpu);
            Gpu = gpu;
        }
    }
}
