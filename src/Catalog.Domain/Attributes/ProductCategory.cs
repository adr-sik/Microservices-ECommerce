using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ProductCategoryAttribute : Attribute
    {
        public ProductType Category { get; }

        public ProductCategoryAttribute(ProductType category)
        {
            Category = category;
        }
    }
}
