using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Creation.Components
{
    public record CreateDisplayRequest(
        ProductType DesignedFor,
        decimal ScreenSizeInches,
        string Resolution,
        int RefreshRateHz,
        string Type = "Display"
        ) : CreateComponentRequest(Type);
}
