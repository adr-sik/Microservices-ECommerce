using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Filtering
{
    public record ProductFilter
    {
        public ProductType Type { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<ComponentFilter>? ComponentFilters { get; set; }
    }
}
