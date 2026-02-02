using Catalog.Application.Factories;
using Catalog.Application.Interfaces;
using Catalog.Application.Objects;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly ProductFactory _productFactory;

        public ProductsService(
            IOptions<CatalogDatabaseSettings> catalogDatabaseSettings,
            ProductFactory productFactory)
        {
            var mongoClient = new MongoClient(
                catalogDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                catalogDatabaseSettings.Value.DatabaseName);
            _productCollection = mongoDatabase.GetCollection<Product>(
                catalogDatabaseSettings.Value.ProductsCollectionName);
            _productFactory = productFactory;
        }

        public async Task<List<Product>> GetAsync() =>
            await _productCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Product> CreateAsync(CreateProductRequest request)
        {
            Product newProduct = await _productFactory.BuildProductAsync(request);
            await _productCollection.InsertOneAsync(newProduct);
            return newProduct;
        }

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);
    }
}
