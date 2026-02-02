using Catalog.Application.Objects;
using Catalog.Application.Strategies;
using Catalog.Domain.Entities;

namespace Catalog.Application.Factories
{
    public class ProductFactory
    {
        private readonly IEnumerable<ICreateProductStrategy> _strategies;

        public ProductFactory(IEnumerable<ICreateProductStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<Product> BuildProductAsync(CreateProductRequest request)
        {
            var strategy = _strategies
                .FirstOrDefault(s => s.GetType().Name.Contains(
                    request.Type, StringComparison.OrdinalIgnoreCase));

            if (strategy == null)
            {
                return null;
                throw new NotSupportedException($"Product type '{request.Type}' is not supported.");               
            }

            return await strategy.CreateProduct(request);
        }
    }
}
