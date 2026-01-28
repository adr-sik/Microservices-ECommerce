using Catalog.Domain.Models.ProductComponents;
using System.ComponentModel;

namespace Catalog.Application.Interfaces
{
    public interface IComponentsService
    {
        Task<List<BaseComponent>> GetAsync();
        Task<BaseComponent?> GetAsync(string id);
        Task CreateAsync(BaseComponent newComponent);
        Task UpdateAsync(string id, BaseComponent updatedComponent);
        Task RemoveAsync(string id);
    }
}
