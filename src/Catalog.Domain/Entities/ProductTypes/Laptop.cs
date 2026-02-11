using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.ProductTypes
{
    public class Laptop : Product
    {
        public Cpu Cpu { get; private set; }
        public Gpu Gpu { get; private set; }
        public Display Display { get; set; }

        public Laptop(
            string brand, string model, decimal price, int stock, string? description, Cpu cpu, Gpu gpu, Display display)
            : base(brand, model, price, stock, description)
        {
            SetCpu(cpu);
            SetGpu(gpu);
            Display = display;
        }

        public void SetCpu(Cpu cpu)
        {
            if (cpu.DesignedFor != Enums.ProductTypes.Laptop)
                throw DomainValidationException.IncompatibleProduct(cpu, this);
            Cpu = cpu;
        }

        public void SetGpu(Gpu gpu)
        {
            if (gpu.DesignedFor != Enums.ProductTypes.Laptop)
                throw DomainValidationException.IncompatibleProduct(gpu, this);
            Gpu = gpu;
        }
    }
}
