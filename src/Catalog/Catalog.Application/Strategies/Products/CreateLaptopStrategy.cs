using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Products;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;

namespace Catalog.Application.Strategies.Products
{
    public class CreateLaptopStrategy : ProductStrategyBase, ICreateProductStrategy<Laptop, CreateLaptopRequest>
    {
        public CreateLaptopStrategy(IComponentsRepository componentsRepository)
            : base(componentsRepository) { }
        public async Task<Product> CreateProduct(CreateProductRequest request)
        {
            if (request is not CreateLaptopRequest laptopRequest)
                throw new ArgumentException("Invalid request type.");

            var cpuTask = GetComponentAsync<Cpu>(laptopRequest.CpuId);
            var gpuTask = GetComponentAsync<Gpu>(laptopRequest.GpuId);
            var displayTask = GetComponentAsync<Display>(laptopRequest.DisplayId);

            await Task.WhenAll(cpuTask, gpuTask, displayTask);

            return new Laptop(
                request.Brand,
                request.Model,
                request.Price,
                request.Description,
                await cpuTask,
                await gpuTask,
                await displayTask
            );
        }

        public async Task<Product> ReplaceProduct(CreateProductRequest request, Product productToUpdate)
        {
            if (productToUpdate is not Laptop laptop)
                throw new InvalidOperationException("Expected Laptop");

            if (request is not CreateLaptopRequest laptopRequest)
                throw new ArgumentException("Invalid request type.");

            var cpuTask = GetComponentAsync<Cpu>(laptopRequest.CpuId);
            var gpuTask = GetComponentAsync<Gpu>(laptopRequest.GpuId);
            var displayTask = GetComponentAsync<Display>(laptopRequest.DisplayId);

            await Task.WhenAll(cpuTask, gpuTask, displayTask);

            laptop.Brand = request.Brand;
            laptop.Model = request.Model;
            laptop.Price = request.Price;
            laptop.Description = request.Description;
            laptop.SetCpu(await cpuTask);
            laptop.SetGpu(await gpuTask);
            laptop.SetDisplay(await displayTask);

            return laptop;
        }
    }
}
