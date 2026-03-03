using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Components
{
    public record CreateGpuRequest(
        ProductType DesignedFor,
        GpuBrand Brand,
        string Model,
        int VRAM,
        string Type = "Gpu"
        ) : CreateComponentRequest(Type);
}
