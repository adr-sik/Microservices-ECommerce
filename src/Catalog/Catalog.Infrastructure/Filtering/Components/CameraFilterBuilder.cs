using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Filtering.Components
{
    internal static class CameraFilterBuilder
    {
        public static FilterDefinition<Product> Build(CameraFilter f)
        {
            var filter = Builders<Product>.Filter.Empty;
            filter &= FilterHelpers.BuildRangeFilter("Camera.Megapixels", f.MinMegapixels, f.MaxMegapixels);

            return filter;
        }
    }
}
