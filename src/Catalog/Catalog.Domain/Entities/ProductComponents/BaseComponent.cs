using Catalog.Domain.Constraints;

namespace Catalog.Domain.Entities.ProductComponents
{
    public abstract class BaseComponent : IIdentityConstraint
    {
        public string Id { get; init; }
    }
}
