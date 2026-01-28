using Catalog.Application.Interfaces;
using Catalog.Domain.Models;
using Catalog.Domain.Models.ProductComponents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Services
{
    public class ComponentsService : IComponentsService
    {
        private readonly IMongoCollection<BaseComponent> _componentCollection;

        public ComponentsService(
            IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                catalogDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                catalogDatabaseSettings.Value.DatabaseName);
            _componentCollection = mongoDatabase.GetCollection<BaseComponent>(
                catalogDatabaseSettings.Value.ComponentsCollectionName);
        }

        public async Task<List<BaseComponent>> GetAsync() =>
            await _componentCollection.Find(_ => true).ToListAsync();

        public async Task<BaseComponent?> GetAsync(string id) =>
            await _componentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(BaseComponent newComponent) =>
            await _componentCollection.InsertOneAsync(newComponent);

        public async Task UpdateAsync(string id, BaseComponent updatedComponent) =>
            await _componentCollection.ReplaceOneAsync(x => x.Id == id, updatedComponent);

        public async Task RemoveAsync(string id) =>
            await _componentCollection.DeleteOneAsync(x => x.Id == id);
    }
}
