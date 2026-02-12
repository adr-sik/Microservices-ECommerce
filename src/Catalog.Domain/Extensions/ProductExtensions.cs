using Catalog.Domain.Attributes;
using Catalog.Domain.Constraints;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using System.Reflection;

namespace Catalog.Domain.Extensions
{
    internal static class ProductExtensions
    {
        public static void ValidateComponent<T>(this Product product, T component)
                where T : IDesignConstraint
        {
            if (component.DesignedFor != product.Type)
                throw DomainValidationException.IncompatibleProduct(component, product);
        }
    }
}
