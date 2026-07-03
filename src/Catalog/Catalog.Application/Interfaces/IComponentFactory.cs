using Catalog.Application.DTOs.Creation;
using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Application.Interfaces
{
    public interface IComponentFactory
    {
        BaseComponent BuildComponent(CreateComponentRequest request);
        BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate);
    }
}
