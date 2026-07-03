using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Products;
using Catalog.Application.Interfaces;
using Catalog.Application.Strategies.Components;
using Catalog.Application.Strategies.Products;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using System.Reflection;

namespace Catalog.Application.Factories
{
    public class ProductFactory : IProductFactory
    {
        private readonly IReadOnlyDictionary<Type, ICreateProductStrategy> _productStrategyMap;
        private readonly IReadOnlyDictionary<Type, ICreateProductStrategy> _requestStrategyMap;

        public ProductFactory(IEnumerable<ICreateProductStrategy> strategies)
        {
            var strategiesList = strategies.ToList();
            _productStrategyMap = BuildProductStrategyMap(strategiesList);
            _requestStrategyMap = BuildRequestStrategyMap(strategiesList);
            ValidateStrategyCompleteness();
        }

        private IReadOnlyDictionary<Type, ICreateProductStrategy> BuildProductStrategyMap(
            IEnumerable<ICreateProductStrategy> strategies)
        {
            var map = new Dictionary<Type, ICreateProductStrategy>();

            foreach (var strategy in strategies)
            {
                var genericInterface = strategy.GetType()
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(ICreateProductStrategy<,>));

                if (genericInterface != null)
                {
                    var productType = genericInterface.GetGenericArguments()[0];
                    map[productType] = strategy;
                }
            }

            return map;
        }

        private IReadOnlyDictionary<Type, ICreateProductStrategy> BuildRequestStrategyMap(
            IEnumerable<ICreateProductStrategy> strategies)
        {
            var map = new Dictionary<Type, ICreateProductStrategy>();

            foreach (var strategy in strategies)
            {
                var genericInterface = strategy.GetType()
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(ICreateProductStrategy<,>));

                if (genericInterface != null)
                {
                    var requestType = genericInterface.GetGenericArguments()[1];
                    map[requestType] = strategy;
                }
            }

            return map;
        }

        private void ValidateStrategyCompleteness()
        {
            var allProductTypes = typeof(Product).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(Product)))
                .ToList();

            var missingStrategies = allProductTypes
                .Where(pt => !_productStrategyMap.ContainsKey(pt))
                .Select(pt => pt.Name)
                .ToList();

            if (missingStrategies.Count > 0)
            {
                throw new ArgumentException(
                    $"Missing strategies for product types: {string.Join(", ", missingStrategies)}. " +
                    $"Ensure all product types have corresponding ICreateProductStrategy<TProduct> implementations.");
            }
        }

        public async Task<Product> BuildProductAsync(CreateProductRequest request)
        {
            var requestType = request.GetType();

            if (!_requestStrategyMap.TryGetValue(requestType, out var strategy))
                throw new NotSupportedException(
                    $"Request type '{requestType.Name}' is not supported.");

            return await strategy.CreateProduct(request);
        }

        public async Task<Product> ReplaceProductAsync(CreateProductRequest request, Product productToUpdate)
        {
            var requestType = productToUpdate.GetType();

            if (!_requestStrategyMap.TryGetValue(requestType, out var strategy))
                throw new NotSupportedException(
                    $"Component type '{request.Type}' is not supported.");

            return await strategy.ReplaceProduct(request, productToUpdate);
        }
    }
}
