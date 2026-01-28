namespace Catalog.Domain.Models.ProductComponents
{
    //[JsonPolymorphic(TypeDiscriminatorPropertyName = "componentType")]
    //[JsonDerivedType(typeof(Cpu<ComputerCpuBrand>), typeDiscriminator: "computerCpu")]
    //[JsonDerivedType(typeof(Cpu<MobileCpuBrand>), typeDiscriminator: "mobileCpu")]
    //[JsonDerivedType(typeof(Gpu<ComputerGpuBrand>), typeDiscriminator: "computerGpu")]
    //[JsonDerivedType(typeof(Gpu<MobileGpuBrand>), typeDiscriminator: "mobileGpu")]
    //[JsonDerivedType(typeof(Display), typeDiscriminator: "display")]
    public abstract class BaseComponent
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
