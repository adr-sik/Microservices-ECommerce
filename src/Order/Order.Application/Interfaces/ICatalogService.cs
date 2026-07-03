using Ordering.Application.DTOs;

namespace Ordering.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<Product>> GetProductsAsync(List<string> ids);
    }
}
