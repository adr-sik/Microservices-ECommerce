using Catalog.Domain.Constraints;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;
using Catalog.Domain.Exceptions;

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
