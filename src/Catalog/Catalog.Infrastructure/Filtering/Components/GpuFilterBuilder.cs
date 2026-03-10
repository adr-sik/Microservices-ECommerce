using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Filtering.Components
{
    internal static class GpuFilterBuilder
    {
        public static FilterDefinition<Product> Build(GpuFilter f)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (f.Brand.HasValue)
                filter &= Builders<Product>.Filter.Eq("Gpu.Brand", f.Brand.Value);
            filter &= FilterHelpers.BuildRangeFilter("Gpu.VRAM", f.MinVRAM, f.MaxVRAM);

            return filter;
        }
    }
}
