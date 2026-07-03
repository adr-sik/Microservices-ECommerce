using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Filtering
{
    internal static class FilterHelpers
    {
        public static FilterDefinition<Product> BuildRangeFilter<T>(
            string fieldPath,
            T? min,
            T? max) where T : struct
        {
            var filter = Builders<Product>.Filter.Empty;

            if (min.HasValue)
                filter &= Builders<Product>.Filter.Gte(fieldPath, min.Value);
            if (max.HasValue)
                filter &= Builders<Product>.Filter.Lte(fieldPath, max.Value);

            return filter;
        }
    }
}
