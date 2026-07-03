using Catalog.Application.DTOs.Creation;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;

namespace Catalog.Application.Strategies.Products
{
    public interface ICreateProductStrategy
    {
        public Task<Product> CreateProduct(CreateProductRequest request);
        public Task<Product> ReplaceProduct(CreateProductRequest request, Product productToUpdate);
    }

    public interface ICreateProductStrategy<TProduct, TRequest> : ICreateProductStrategy 
        where TProduct : Product
        where TRequest : CreateProductRequest
    {
    }
}
