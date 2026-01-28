using Catalog.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Services
{
    public class ProductsService
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductsService(
            IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                catalogDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                catalogDatabaseSettings.Value.DatabaseName);
            _productCollection = mongoDatabase.GetCollection<Product>(
                catalogDatabaseSettings.Value.ProductsCollectionName);
        }

        //TODO: rest of CRUD operations
        public async Task<List<Product>> GetAsync() =>
            await _productCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);
    }
}
