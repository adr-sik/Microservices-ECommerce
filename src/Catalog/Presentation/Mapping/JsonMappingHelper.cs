using Catalog.Domain.Models;
using Catalog.Domain.Models.Enums;
using Catalog.Domain.Models.ProductComponents;
using Catalog.Domain.Models.ProductTypes;
using System.Text.Json.Serialization.Metadata;

namespace Catalog.Presentation.Mapping
{
    public static class JsonMappingHelper
    {
        public static void ConfigureJsonPolymorphism(JsonTypeInfo jsonTypeInfo)
        {
            // Product polymorphism configuration
            if (jsonTypeInfo.Type == typeof(Product))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "productType",
                    DerivedTypes =
                    {
                        new JsonDerivedType(typeof(Laptop), "laptop"),
                        new JsonDerivedType(typeof(Phone), "phone")
                    }
                };
            }

            // Component polymorphism configuration
            if (jsonTypeInfo.Type == typeof(BaseComponent))
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "componentType",
                    DerivedTypes =
                    {
                        new JsonDerivedType(typeof(Cpu<ComputerCpuBrand>), "computerCpu"),
                        new JsonDerivedType(typeof(Gpu<ComputerGpuBrand>), "computerGpu"),
                        new JsonDerivedType(typeof(Cpu<MobileCpuBrand>), "mobileCpu"),
                        new JsonDerivedType(typeof(Gpu<MobileGpuBrand>), "mobileGpu"),
                        new JsonDerivedType(typeof(Display), "display"),
                        new JsonDerivedType(typeof(Camera), "camera")
                    }
                };
            }
        }
    }
}
