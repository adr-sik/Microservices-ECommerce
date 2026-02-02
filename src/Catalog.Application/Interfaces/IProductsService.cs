using Catalog.Application.Objects;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces
{
    public interface IProductsService
    {
        Task<List<Product>> GetAsync();
        Task<Product?> GetAsync(string id);
        Task<Product> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(string id, Product updatedProduct);
        Task RemoveAsync(string id);
    }
}
