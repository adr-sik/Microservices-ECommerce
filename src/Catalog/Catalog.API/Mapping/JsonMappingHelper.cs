using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Filtering.Components;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using System.Text.Json.Serialization.Metadata;

namespace Catalog.API.Mapping
{
    public static class JsonMappingHelper
    {
        public static void ConfigureJsonPolymorphism(JsonTypeInfo jsonTypeInfo)
        {
            var roots = new[] {
            typeof(Product),
            typeof(BaseComponent),
            typeof(ComponentFilter),
            typeof(CreateProductRequest),
            typeof(CreateComponentRequest)
        };

            if (!roots.Contains(jsonTypeInfo.Type)) return;

            var options = new JsonPolymorphismOptions { TypeDiscriminatorPropertyName = "type" };

            var derivedTypes = jsonTypeInfo.Type.Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && jsonTypeInfo.Type.IsAssignableFrom(t));

            foreach (var type in derivedTypes)
            {
                var name = type.Name
                    .Replace("Create", "")
                    .Replace("Request", "")
                    .Replace("Filter", "")
                    .Replace("Dto", "");    

                options.DerivedTypes.Add(new JsonDerivedType(type, name));
            }

            jsonTypeInfo.PolymorphismOptions = options;
        }
    }
}
