using Catalog.Domain.Constraints;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductComponents
{
    public class Camera : BaseComponent, IDesignConstraint
    {
        public required ProductType DesignedFor { get; set; }
        public int Megapixels { get; set; }
    }
}
