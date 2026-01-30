namespace Catalog.Domain.Entities.ProductComponents
{
    public class Gpu<TBrand> : BaseComponent where TBrand : Enum
    {
        public required TBrand Brand { get; set; }
        public required string Model { get; set; }
        public int VRAM { get; set; }
    }
}
