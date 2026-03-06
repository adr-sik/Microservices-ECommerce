using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Components
{
    public record CreateCameraRequest(
        ProductType DesignedFor,
        int Megapixels,
        string Type = "Camera"
        ) : CreateComponentRequest(Type);
}
