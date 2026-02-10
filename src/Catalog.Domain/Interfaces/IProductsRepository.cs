using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAsync();
        Task<Product?> GetAsync(string id);
        Task CreateAsync(Product newProduct);
        Task UpdateAsync(string id, Product updatedProduct);
        Task RemoveAsync(string id);
    }
}
