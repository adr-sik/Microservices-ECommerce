using Catalog.Domain.Constraints;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.ProductComponents
{
    public abstract class BaseComponent : IIdentityConstraint
    {
        public string Id { get; init; }
    }
}
