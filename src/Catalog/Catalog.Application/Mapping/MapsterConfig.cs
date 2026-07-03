using Catalog.Application.DTOs.ReadOnly;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Mapster;

namespace Catalog.Application.Mapping
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<BaseComponent, ComponentDto>.NewConfig()
                .Map(dest => dest.Type, src => src.GetType().Name.Replace("Component", "").ToLower())
                .Map(dest => dest.Specifications, src => src.UniqueAttributesToMetadata());

            TypeAdapterConfig<Product, ProductDto>.NewConfig()
                //.Map(dest => dest.Type, src => src.Type.ToString())
                .Map(dest => dest.Components, src => src.GetProductComponents());
        }
    }
}
