namespace Catalog.Domain.Entities.ProductComponents
{
    public class Cpu<TBrand> : BaseComponent where TBrand : Enum
    {
        public required TBrand Brand { get; set; }
        public required string Model { get; set; }
        public int NumberOfCores { get; set; }
    }
}
