using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Pagination;
using Catalog.Application.DTOs.Sorting;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;

namespace Catalog.Application.Interfaces
{
    public interface IProductsRepository
    {
        Task<(IReadOnlyCollection<Product> Items, int TotalCount)> GetAsync(PaginationRequest pagination, ProductSortRequest? sort);
        Task<(IReadOnlyCollection<Product> Items, int TotalCount)> GetAsync(ProductFilter filter, PaginationRequest pagination, ProductSortRequest? sort);
        Task<(IReadOnlyCollection<Product> Items, int TotalCount)> GetByTypeAsync(ProductType type, PaginationRequest pagination, ProductSortRequest? sort);
        Task<Product?> GetAsync(string id);
        Task CreateAsync(Product product, CancellationToken ct = default);
        Task UpdateAsync(string id, Product updatedProduct);
        Task RemoveAsync(string id);
        Task<List<Product>> GetAllByIdAsync(List<string> ids);
    }
}
