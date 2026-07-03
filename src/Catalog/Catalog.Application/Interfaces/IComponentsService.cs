using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.ReadOnly;

namespace Catalog.Application.Interfaces
{
    public interface IComponentsService
    {
        Task<IReadOnlyList<ComponentDto>> GetAsync();
        Task<ComponentDto?> GetAsync(string id);
        Task<ComponentDto> CreateAsync(CreateComponentRequest newComponent);
        Task UpdateAsync(string id, CreateComponentRequest updatedComponent);
        Task RemoveAsync(string id);
    }
}
