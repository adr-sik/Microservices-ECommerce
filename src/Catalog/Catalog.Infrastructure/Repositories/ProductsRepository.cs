using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Pagination;
using Catalog.Application.DTOs.Sorting;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Catalog.Infrastructure.Filtering;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Sorting;
using Catalog.Infrastructure.Transactions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly CatalogMongoDbContext _context;
        private readonly ITransactionIdProvider<IClientSessionHandle> _sessionProvider;
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(
            CatalogMongoDbContext context,
            ITransactionIdProvider<IClientSessionHandle> sessionProvider,
            ILogger<ProductsRepository> logger)
        {
            _context = context;
            _sessionProvider = sessionProvider;
            _logger = logger;
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

        public async Task CreateAsync(Product product, CancellationToken ct)
        {
            var session = _sessionProvider.TransactionHandler;

            if (session != null)
                await _context.Products.InsertOneAsync(session, product, cancellationToken: ct);
            else
                await _context.Products.InsertOneAsync(product, cancellationToken: ct);
        }

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
