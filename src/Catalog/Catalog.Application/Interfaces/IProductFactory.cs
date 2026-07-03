using Catalog.Application.DTOs.Creation;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces
{
    public interface IProductFactory
    {
        Task<Product> BuildProductAsync(CreateProductRequest request);
        Task<Product> ReplaceProductAsync(CreateProductRequest request, Product productToUpdate);
    }
}
