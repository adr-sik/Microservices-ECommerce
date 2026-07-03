using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using System.Collections.Concurrent;
using System.Reflection;

namespace Catalog.Application.Mapping
{
    public static class EntityMetadataExtensions
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> AttributeCache = new();
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> ComponentCache = new();
        public static Dictionary<string, object> UniqueAttributesToMetadata(this BaseComponent component)
        {
            var type = component.GetType();
            var distinctProperties = AttributeCache.GetOrAdd(type, t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.DeclaringType != typeof(BaseComponent))
                .ToArray());

            var metadata = new Dictionary<string, object>();
            foreach (var prop in distinctProperties)
            {
                var value = prop.GetValue(component);
                if (value != null)
                {
                    metadata[prop.Name] = value;
                }
            }

            return metadata;
        }

        public static List<BaseComponent> GetProductComponents(this Product product)
        {
            var type = product.GetType();
            var componentProperties = ComponentCache.GetOrAdd(type, t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(BaseComponent).IsAssignableFrom(p.PropertyType))
                .ToArray());

            var components = new List<BaseComponent>();
            foreach (var prop in componentProperties)
            {
                if (prop.GetValue(product) is BaseComponent comp)
                {
                    components.Add(comp);
                }
            }

            return components;
        }
    }
}
