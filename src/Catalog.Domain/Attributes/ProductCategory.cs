using Catalog.Domain.Enums;

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
