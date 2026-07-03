using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Components
{
    public record CreateCpuRequest(
        ProductType DesignedFor,
        CpuBrand Brand,
        string Model,
        int NumberOfCores,
        string Type = "Cpu"
        ) : CreateComponentRequest(Type);
}
