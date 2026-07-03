using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;
using Catalog.Domain.Extensions;

namespace Catalog.Domain.Entities.ProductTypes
{
    public class Laptop : Product
    {
        public override ProductType Type => ProductType.Laptop;
        public Cpu Cpu { get; private set; }
        public Gpu Gpu { get; private set; }
        public Display Display { get; private set; }

        public Laptop(
            string brand, string model, decimal price, string? description, Cpu cpu, Gpu gpu, Display display)
            : base(brand, model, price, description)
        {
            SetCpu(cpu);
            SetGpu(gpu);
            SetDisplay(display);
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

        public void SetDisplay(Display display)
        {
            this.ValidateComponent(display);
            Display = display;
        }
    }
}
