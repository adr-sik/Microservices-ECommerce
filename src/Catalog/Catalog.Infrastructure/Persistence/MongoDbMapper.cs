using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalog.Infrastructure.Persistence
{
    public static class MongoDbMapper
    {
        private static bool _isMapped = false;

        public static void MapClasses()
        {
            if (_isMapped) return;

            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));

            BsonSerializer.RegisterSerializer(new EnumSerializer<CpuBrand>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<GpuBrand>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<ProductType>(BsonType.String));

            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.SetIsRootClass(true);

                MapDerivedClasses<Product>(cm);
            });

            BsonClassMap.RegisterClassMap<BaseComponent>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.SetIsRootClass(true);
            });

            _isMapped = true;
        }

        private static void MapDerivedClasses<TBase>(BsonClassMap cm) where TBase : class
        {
            var derivedTypes = typeof(TBase).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(TBase)));

            foreach (var derivedType in derivedTypes)
            {
                cm.AddKnownType(derivedType);
            }
        }
    }
}
