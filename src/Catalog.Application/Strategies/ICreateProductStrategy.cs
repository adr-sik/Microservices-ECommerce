using Catalog.Application.Objects;
using Catalog.Domain.Entities;

namespace Catalog.Application.Strategies
{
    public interface ICreateProductStrategy
    {
        public Task<Product> CreateProduct(CreateProductRequest request);
    }
}
