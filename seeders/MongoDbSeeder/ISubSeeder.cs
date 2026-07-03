using Catalog.Application.DTOs.ReadOnly;
using Catalog.Domain.Entities.ProductComponents;

namespace MongoDbSeeder
{
    public interface ISubSeeder
    {
        Task SeedAsync(List<BaseComponent> generatedComponents);
    }
}
