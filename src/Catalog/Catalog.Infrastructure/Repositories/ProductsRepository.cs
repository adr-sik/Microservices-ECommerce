using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly MongoDbContext _context;

        public ProductsRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Product>> GetAsync() =>
            await _context.Products.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _context.Products.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _context.Products.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _context.Products.DeleteOneAsync(x => x.Id == id);

    }
}
