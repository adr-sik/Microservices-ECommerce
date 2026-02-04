using Catalog.Application.Interfaces;
using Catalog.Application.Objects;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;

namespace Catalog.Application.Strategies
{
    public class CreateLaptopStrategy : ICreateProductStrategy
    {
        private readonly IComponentsService _componentsService;

        public CreateLaptopStrategy(IComponentsService componentsService)
        {
            _componentsService = componentsService;
        }

        public async Task<Product> CreateProduct(CreateProductRequest request)
        {
            try
            {
                var cpuId = request.Components != null && request.Components.ContainsKey("CpuId")
                    ? request.Components["CpuId"]
                    : throw new ArgumentException("CpuId is required for creating a Laptop.");

                var gpuId = request.Components != null && request.Components.ContainsKey("GpuId")
                    ? request.Components["GpuId"]
                    : throw new ArgumentException("GpuId is required for creating a Laptop.");

                var displayId = request.Components != null && request.Components.ContainsKey("DisplayId")
                    ? request.Components["DisplayId"]
                    : throw new ArgumentException("DisplayId is required for creating a Laptop.");

                Laptop newLaptop = new Laptop
                {
                    Brand = request.Brand,
                    Model = request.Model,
                    Price = request.Price,
                    Stock = request.Stock,
                    Description = request.Description,
                    Cpu = (Cpu<ComputerCpuBrand>)await _componentsService.GetAsync(cpuId),
                    Gpu = (Gpu<ComputerGpuBrand>)await _componentsService.GetAsync(gpuId),
                    Display = (Display)await _componentsService.GetAsync(displayId)
                };

                return newLaptop;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
