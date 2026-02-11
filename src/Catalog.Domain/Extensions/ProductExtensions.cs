using Catalog.Domain.Attributes;
using Catalog.Domain.Constraints;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;
using Catalog.Domain.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Extensions
{
    internal static class ProductExtensions
    {
        public static void ValidateComponent<T>(this Product product, T component)
                where T : IDesignConstraint
        {
            var productType = product.GetType().GetCustomAttribute<ProductCategoryAttribute>()?.Category
                ?? throw new InvalidOperationException($"Product {product.GetType().Name} does not have a ProductCategoryAttribute");

            if (component.DesignedFor != productType)
                throw DomainValidationException.IncompatibleProduct(component, product);
        }
    }
}
