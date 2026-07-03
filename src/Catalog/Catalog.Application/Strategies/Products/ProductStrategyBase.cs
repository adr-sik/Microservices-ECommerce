using Catalog.Application.Interfaces;
using Catalog.Domain.Constraints;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Exceptions;

namespace Catalog.Application.Strategies.Products
{
    public class ProductStrategyBase
    {
        protected readonly IComponentsRepository _componentsRepository;
        public ProductStrategyBase(IComponentsRepository componentsRepository)
        {
            _componentsRepository = componentsRepository;
        }

        protected async Task<T> GetComponentAsync<T>(string id) where T : BaseComponent
        {
            var component = await _componentsRepository.GetAsync(id);

            if (component == null)
                throw DomainReferenceException.NotFound(id, typeof(T).Name);

            if (component is not T typedComponent)
                throw DomainReferenceException.TypeMismatch(component as IIdentityConstraint, typeof(T).Name, component.GetType().Name);

            return typedComponent;
        }
    }
}
