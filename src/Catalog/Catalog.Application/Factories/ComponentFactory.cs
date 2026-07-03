using Catalog.Application.DTOs.Creation;
using Catalog.Application.Interfaces;
using Catalog.Application.Strategies.Components;
using Catalog.Domain.Entities.ProductComponents;
using System.Reflection;

namespace Catalog.Application.Factories
{
    public class ComponentFactory : IComponentFactory
    {
        private readonly IReadOnlyDictionary<Type, ICreateComponentStrategy> _componentStrategyMap;
        private readonly IReadOnlyDictionary<Type, ICreateComponentStrategy> _requestStrategyMap;

        public ComponentFactory(IEnumerable<ICreateComponentStrategy> strategies)
        {
            var strategiesList = strategies.ToList();
            _componentStrategyMap = BuildComponentStrategyMap(strategiesList);
            _requestStrategyMap = BuildRequestStrategyMap(strategiesList);
            ValidateStrategyCompleteness();
        }

        private IReadOnlyDictionary<Type, ICreateComponentStrategy> BuildComponentStrategyMap(
            IEnumerable<ICreateComponentStrategy> strategies)
        {
            var map = new Dictionary<Type, ICreateComponentStrategy>();

            foreach (var strategy in strategies)
            {
                var genericInterface = strategy.GetType()
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(ICreateComponentStrategy<,>));

                if (genericInterface != null)
                {
                    var componentType = genericInterface.GetGenericArguments()[0];
                    map[componentType] = strategy;
                }
            }

            return map;
        }

        private IReadOnlyDictionary<Type, ICreateComponentStrategy> BuildRequestStrategyMap(
            IEnumerable<ICreateComponentStrategy> strategies)
        {
            var map = new Dictionary<Type, ICreateComponentStrategy>();

            foreach (var strategy in strategies)
            {
                var genericInterface = strategy.GetType()
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(ICreateComponentStrategy<,>));

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
            var allComponentTypes = typeof(BaseComponent).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(BaseComponent)))
                .ToList();

            var missingStrategies = allComponentTypes
                .Where(ct => !_componentStrategyMap.ContainsKey(ct))
                .Select(ct => ct.Name)
                .ToList();

            if (missingStrategies.Count > 0)
            {
                throw new ArgumentException(
                    $"Missing strategies for component types: {string.Join(", ", missingStrategies)}. " +
                    $"Ensure all component types have corresponding ICreateComponentStrategy<TComponent, TRequest> implementations.");
            }
        }

        public BaseComponent BuildComponent(CreateComponentRequest request)
        {
            var requestType = request.GetType();

            if (!_requestStrategyMap.TryGetValue(requestType, out var strategy))
                throw new NotSupportedException($"No strategy found for request type '{requestType.Name}'.");

            return strategy.CreateComponent(request);
        }

        public BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate)
        {
            var componentType = componentToUpdate.GetType();

            if (!_componentStrategyMap.TryGetValue(componentType, out var strategy))
                throw new NotSupportedException($"No strategy found for component type '{componentType.Name}'.");

            return strategy.ReplaceComponent(request, componentToUpdate);
        }
    }
}
