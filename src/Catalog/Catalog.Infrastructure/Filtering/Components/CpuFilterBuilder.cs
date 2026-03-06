using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Filtering.Components
{
    internal static class CpuFilterBuilder
    {
        public static FilterDefinition<Product> Build(CpuFilter f)
        {
            var filter = Builders<Product>.Filter.Empty;
            if (f.Brand.HasValue)
                filter &= Builders<Product>.Filter.Eq("Cpu.Brand", f.Brand.Value);
            filter &= FilterHelpers.BuildRangeFilter("Cpu.NumberOfCores", f.MinNumberOfCores, f.MaxNumberOfCores);

            return filter;
        }
    }
}
