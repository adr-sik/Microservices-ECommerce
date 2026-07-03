using Catalog.Application.DTOs.Creation;
using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Application.Strategies.Components
{
    public interface ICreateComponentStrategy
    {
        BaseComponent CreateComponent(CreateComponentRequest request);
        BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate);
    }

    public interface ICreateComponentStrategy<TComponent, TRequest> : ICreateComponentStrategy
        where TComponent : BaseComponent
        where TRequest : CreateComponentRequest
    {
    }
}
