using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Components;
using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Application.Strategies.Components
{
    public class CreateGpuStrategy : ICreateComponentStrategy<Gpu, CreateGpuRequest>
    {
        public BaseComponent CreateComponent(CreateComponentRequest request)
        {
            if (request is not CreateGpuRequest gpuRequest)
                throw new ArgumentException("Invalid request type.");

            return new Gpu
            {
                DesignedFor = gpuRequest.DesignedFor,
                Brand = gpuRequest.Brand,
                Model = gpuRequest.Model,
                VRAM = gpuRequest.VRAM
            };
        }

        public BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate)
        {
            if (componentToUpdate is not Gpu gpu)
                throw new InvalidOperationException("Expected Gpu");

            if (request is not CreateGpuRequest gpuRequest)
                throw new ArgumentException("Invalid request type.");

            gpu.DesignedFor = gpuRequest.DesignedFor;
            gpu.Brand = gpuRequest.Brand;
            gpu.Model = gpuRequest.Model;
            gpu.VRAM = gpuRequest.VRAM;

            return gpu;
        }
    }
}
