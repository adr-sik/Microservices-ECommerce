using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Filtering.Components;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Filtering
{
    public static class ProductFilterBuilder
    {
        public static FilterDefinition<Product> BuildFilter(ProductFilter filter)
        {
            ValidateComponentFilters(filter.ComponentFilters);

            var builder = Builders<Product>.Filter;
            var filterDefinition = builder.Empty;

            filterDefinition &= builder.Eq("_t", filter.Type);
            if (!string.IsNullOrEmpty(filter.Brand))
                filterDefinition &= builder.Eq(p => p.Brand, filter.Brand);
            if (!string.IsNullOrEmpty(filter.Model))
                filterDefinition &= builder.Eq(p => p.Model, filter.Model);
            if (filter.MinPrice.HasValue)
                filterDefinition &= builder.Gte(p => p.Price, filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue)
                filterDefinition &= builder.Lte(p => p.Price, filter.MaxPrice.Value);

            foreach (var componentFilter in filter.ComponentFilters)
            {
                filterDefinition &= componentFilter switch
                {
                    CpuFilter cpu => CpuFilterBuilder.Build(cpu),
                    GpuFilter gpu => GpuFilterBuilder.Build(gpu),
                    DisplayFilter display => DisplayFilterBuilder.Build(display),
                    CameraFilter camera => CameraFilterBuilder.Build(camera),
                    _ => Builders<Product>.Filter.Empty
                };
            }

            return filterDefinition;
        }

        private static void ValidateComponentFilters(List<ComponentFilter>? filters)
        {
            if (filters == null) return;

            var duplicates = filters
                .GroupBy(f => f.GetType())
                .Where(g => g.Count()>1)
                .Select(g => g.Key.Name)
                .ToList();

            if (duplicates.Any())
                throw new ArgumentException(
                    $"Only one filter per component type is allowed: {string.Join(", ", duplicates)}");
        }
    }
}
