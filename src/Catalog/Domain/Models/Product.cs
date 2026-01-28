namespace Catalog.Domain.Models
{
    //[JsonPolymorphic(TypeDiscriminatorPropertyName = "productType")]
    //[JsonDerivedType(typeof(Laptop), typeDiscriminator: "laptop")]
    //[JsonDerivedType(typeof(Phone), typeDiscriminator: "phone")]
    //[BsonDiscriminator(RootClass = true)]
    //[BsonKnownTypes(typeof(Laptop), typeof(Phone))]
    public abstract class Product
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Description { get; set; }
    }
}
