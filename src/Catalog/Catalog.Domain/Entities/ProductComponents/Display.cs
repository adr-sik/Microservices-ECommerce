using Catalog.Domain.Constraints;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities.ProductComponents
{
    public class Display : BaseComponent, IDesignConstraint
    {
        public required ProductType DesignedFor { get; set; }
        private decimal _screenSize;
        public decimal ScreenSizeInches
        {
            get => _screenSize;
            set => _screenSize = Math.Round(value, 1);
        }
        public required string Resolution { get; set; }
        public int RefreshRateHz { get; set; }
    }
}
