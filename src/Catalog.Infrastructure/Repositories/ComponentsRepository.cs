using Catalog.Application.Interfaces;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ComponentsRepository : IComponentsRepository
    {
        private readonly MongoDbContext _dbContext;

        public ComponentsRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<BaseComponent>> GetAsync() =>
            await _dbContext.Components.Find(_ => true).ToListAsync();

        public async Task<BaseComponent?> GetAsync(string id) =>
            await _dbContext.Components.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(BaseComponent newComponent) =>
            await _dbContext.Components.InsertOneAsync(newComponent);

        public async Task UpdateAsync(string id, BaseComponent updatedComponent) =>
            await _dbContext.Components.ReplaceOneAsync(x => x.Id == id, updatedComponent);

        public async Task RemoveAsync(string id) =>
            await _dbContext.Components.DeleteOneAsync(x => x.Id == id);
    }
}
