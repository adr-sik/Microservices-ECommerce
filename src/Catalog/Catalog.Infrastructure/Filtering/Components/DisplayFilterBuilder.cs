using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Filtering.Components
{
    internal static class DisplayFilterBuilder
    {
        public static FilterDefinition<Product> Build(DisplayFilter f)
        {
            var filter = Builders<Product>.Filter.Empty;

            filter &= FilterHelpers.BuildRangeFilter("Display.ScreenSizeInches", f.MinScreenSizeInches, f.MaxScreenSizeInches);
            filter &= FilterHelpers.BuildRangeFilter("Display.RefreshRateHz", f.MinRefreshRateHz, f.MaxRefreshRateHz);
            if (!string.IsNullOrEmpty(f.Resolution))
                filter &= Builders<Product>.Filter.Eq("Display.Resolution", f.Resolution);

            return filter;
        }
    }
}
