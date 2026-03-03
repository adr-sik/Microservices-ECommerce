using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Pagination;
using Catalog.Application.DTOs.Sorting;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Catalog.Infrastructure.Filtering;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Sorting;
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

        public async Task<(IReadOnlyCollection<Product> Items, int TotalCount)> GetAsync(
            PaginationRequest pagination, ProductSortRequest? sort)
        {
            var totalCount = await _context.Products.CountDocumentsAsync(_ => true);
            var query = _context.Products.Find(_ => true);

            if (sort is not null)
                query = query.Sort(ProductSorterBuilder.BuildSort(sort));

            var items = await query
                .Skip(pagination.Skip)
                .Limit(pagination.Take)
                .ToListAsync();

            return (items, (int)totalCount);
        }

        public async Task<Product?> GetAsync(string id) =>
            await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<(IReadOnlyCollection<Product> Items, int TotalCount)> GetAsync(
            ProductFilter filter, PaginationRequest pagination, ProductSortRequest? sort)
        {
            var mongoFilter = ProductFilterBuilder.BuildFilter(filter);
            var totalCount = await _context.Products.CountDocumentsAsync(mongoFilter);
            var query = _context.Products.Find(mongoFilter);

            if (sort is not null)
                query = query.Sort(ProductSorterBuilder.BuildSort(sort));

            var items = await query
                .Skip(pagination.Skip)
                .Limit(pagination.Take)
                .ToListAsync();

            return (items, (int)totalCount);
        }

        public async Task<(IReadOnlyCollection<Product> Items, int TotalCount)> GetByTypeAsync(
            ProductType type, PaginationRequest pagination, ProductSortRequest? sort)
        {
            var mongoFilter = Builders<Product>.Filter.AnyEq("_t", type);
            var totalCount = await _context.Products.CountDocumentsAsync(mongoFilter);
            var query = _context.Products.Find(mongoFilter);

            if (sort is not null)
                query = query.Sort(ProductSorterBuilder.BuildSort(sort));

            var items = await query
                .Skip(pagination.Skip)
                .Limit(pagination.Take)
                .ToListAsync();

            return (items, (int)totalCount);
        }

        public async Task CreateAsync(Product newProduct) =>
            await _context.Products.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _context.Products.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _context.Products.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Product>> GetAllByIdAsync(List<string> ids)
        {
            var filter = Builders<Product>.Filter.In(p => p.Id, ids);

            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }
    }
}
