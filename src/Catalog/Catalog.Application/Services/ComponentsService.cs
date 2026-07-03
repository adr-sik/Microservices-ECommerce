using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.Interfaces;
using Mapster;

namespace Catalog.Application.Services
{
    public class ComponentsService : IComponentsService
    {
        private readonly IComponentsRepository _componentsRepository;
        private readonly IComponentFactory _componentFactory;
        public ComponentsService(IComponentsRepository componentsRepository,
            IComponentFactory componentFactory)
        {
            _componentsRepository = componentsRepository;
            _componentFactory = componentFactory;
        }

        public async Task<IReadOnlyList<ComponentDto>> GetAsync()
        {
            var components = await _componentsRepository.GetAsync();
            return components.Adapt<IReadOnlyList<ComponentDto>>();
        }

        public async Task<ComponentDto?> GetAsync(string id)
        {
            var component = await _componentsRepository.GetAsync(id);
            return component.Adapt<ComponentDto>();
        }

        public async Task<ComponentDto> CreateAsync(CreateComponentRequest request)
        {
            var newComponent = _componentFactory.BuildComponent(request);
            await _componentsRepository.CreateAsync(newComponent);
            return newComponent.Adapt<ComponentDto>();
        }

        public async Task UpdateAsync(string id, CreateComponentRequest request)
        {
            var componentToUpdate = await _componentsRepository.GetAsync(id);
            var updatedComponent = _componentFactory.ReplaceComponent(request, componentToUpdate);
            await _componentsRepository.UpdateAsync(id, updatedComponent);
        }

        public async Task RemoveAsync(string id) =>
            await _componentsRepository.RemoveAsync(id);
    }
}
