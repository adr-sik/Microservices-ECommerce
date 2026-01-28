using Catalog.Application.Objects;
using Catalog.Domain.Models;

namespace Catalog.Application.Strategies
{
    public interface ICreateProductStrategy
    {
        public Task<Product> CreateProduct(CreateProductRequest request);
    }
}
