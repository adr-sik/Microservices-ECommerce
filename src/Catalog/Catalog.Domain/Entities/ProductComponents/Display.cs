using Catalog.Domain.Constraints;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductComponents
{
    public class Display : BaseComponent, IDesignConstraint
    {
        public required ProductType DesignedFor { get; set; }
        public double ScreenSizeInches { get; set; }
        public required string Resolution { get; set; }
        public int RefreshRateHz { get; set; }
    }
}
