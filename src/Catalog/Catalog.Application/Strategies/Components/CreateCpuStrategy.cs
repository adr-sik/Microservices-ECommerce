using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Components;
using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Application.Strategies.Components
{
    public class CreateCpuStrategy : ICreateComponentStrategy<Cpu, CreateCpuRequest>
    {
        public BaseComponent CreateComponent(CreateComponentRequest request)
        {
            if (request is not CreateCpuRequest cpuRequest)
                throw new ArgumentException("Invalid request type.");

            return new Cpu
            {
                DesignedFor = cpuRequest.DesignedFor,
                Brand = cpuRequest.Brand,
                Model = cpuRequest.Model,
                NumberOfCores = cpuRequest.NumberOfCores
            };
        }

        public BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate)
        {
            if (componentToUpdate is not Cpu cpu)
                throw new InvalidOperationException("Expected Cpu");

            if (request is not CreateCpuRequest cpuRequest)
                throw new ArgumentException("Invalid request type.");

            cpu.DesignedFor = cpuRequest.DesignedFor;
            cpu.Brand = cpuRequest.Brand;
            cpu.Model = cpuRequest.Model;
            cpu.NumberOfCores = cpuRequest.NumberOfCores;

            return cpu;
        }
    }
}
