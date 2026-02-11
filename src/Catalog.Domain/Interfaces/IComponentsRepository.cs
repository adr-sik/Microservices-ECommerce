using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Domain.Interfaces
{
    public interface IComponentsRepository
    {
        Task<IReadOnlyCollection<BaseComponent>> GetAsync();
        Task<BaseComponent?> GetAsync(string id);
        Task CreateAsync(BaseComponent newComponent);
        Task UpdateAsync(string id, BaseComponent updatedComponent);
        Task RemoveAsync(string id);
    }
}
