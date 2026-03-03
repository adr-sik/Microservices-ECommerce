using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence
{
    public class MongoDbContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<BaseComponent> Components { get; }

        public MongoDbContext(IOptions<CatalogDatabaseSettings> settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Products = database.GetCollection<Product>(settings.Value.ProductsCollectionName);
            Components = database.GetCollection<BaseComponent>(settings.Value.ComponentsCollectionName);
        }
    }
}
