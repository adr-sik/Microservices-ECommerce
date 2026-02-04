namespace Catalog.Domain.Models.ProductComponents
{
    public class Display : BaseComponent
    {
        public double ScreenSizeInches { get; set; }
        public required string Resolution { get; set; }
        public int RefreshRateHz { get; set; }
    }
}
