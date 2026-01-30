using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalog.Infrastructure.Persistence
{
    // I don't know if this is a good idea
    public static class MongoDbMapper
    {
        private static bool _isMapped = false;

        public static void MapClasses()
        {
            if (_isMapped) return;

            BsonSerializer.RegisterSerializer(new EnumSerializer<ComputerCpuBrand>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<ComputerGpuBrand>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<MobileCpuBrand>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<MobileGpuBrand>(BsonType.String));

            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.SetIsRootClass(true);

                cm.MapMember(p => p.Price).SetSerializer(new DecimalSerializer(BsonType.Decimal128));

                cm.AddKnownType(typeof(Laptop));
                cm.AddKnownType(typeof(Phone));
            });

            BsonClassMap.RegisterClassMap<BaseComponent>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.SetIsRootClass(true);

                cm.AddKnownType(typeof(Cpu<ComputerCpuBrand>));
                cm.AddKnownType(typeof(Gpu<ComputerGpuBrand>));
                cm.AddKnownType(typeof(Cpu<MobileCpuBrand>));
                cm.AddKnownType(typeof(Gpu<MobileGpuBrand>));
                cm.AddKnownType(typeof(Display));
                cm.AddKnownType(typeof(Camera));

            });

            _isMapped = true;
        }
    }
}
