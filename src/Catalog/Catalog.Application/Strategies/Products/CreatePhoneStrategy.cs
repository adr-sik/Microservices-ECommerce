using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Products;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;

namespace Catalog.Application.Strategies.Products
{
    public class CreatePhoneStrategy : ProductStrategyBase, ICreateProductStrategy<Phone, CreatePhoneRequest>
    {
        public CreatePhoneStrategy(IComponentsRepository componentsRepository) 
            : base(componentsRepository) { }

        public async Task<Product> CreateProduct(CreateProductRequest request)
        {
            if (request is not CreatePhoneRequest phoneRequest)
                throw new ArgumentException("Invalid request type.");

            var cpuTask = GetComponentAsync<Cpu>(phoneRequest.CpuId);
            var gpuTask = GetComponentAsync<Gpu>(phoneRequest.GpuId);
            var displayTask = GetComponentAsync<Display>(phoneRequest.DisplayId);
            var cameraTask = GetComponentAsync<Camera>(phoneRequest.CameraId);

            await Task.WhenAll(cpuTask, gpuTask, displayTask, cameraTask);

            return new Phone(
                request.Brand,
                request.Model,
                request.Price,
                request.Description,
                await cpuTask,
                await gpuTask,
                await displayTask,
                await cameraTask
            );
        }

        public async Task<Product> ReplaceProduct(CreateProductRequest request, Product productToUpdate)
        {
            if (productToUpdate is not Phone phone)
                throw new InvalidOperationException("Expected Phone");

            if (request is not CreatePhoneRequest phoneRequest)
                throw new ArgumentException("Invalid request type.");

            var cpuTask = GetComponentAsync<Cpu>(phoneRequest.CpuId);
            var gpuTask = GetComponentAsync<Gpu>(phoneRequest.GpuId);
            var displayTask = GetComponentAsync<Display>(phoneRequest.DisplayId);
            var cameraTask = GetComponentAsync<Camera>(phoneRequest.CameraId);

            await Task.WhenAll(cpuTask, gpuTask, displayTask, cameraTask);

            phone.Brand = request.Brand;
            phone.Model = request.Model;
            phone.Price = request.Price;
            phone.Description = request.Description;
            phone.SetCpu(await cpuTask);
            phone.SetGpu(await gpuTask);
            phone.SetDisplay(await displayTask);
            phone.SetCamera(await cameraTask);

            return phone;
        }
    }
}
