using Catalog.Domain.Constraints;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.ProductComponents
{
    public abstract class BaseComponent : IIdentityConstraint
    {
        public string Id { get; protected set; }

        //public BaseComponent(string id) => Id = id;

        public void SetIdentity(string id)
        {
            if (!string.IsNullOrEmpty(this.Id))
                throw DomainValidationException.IdentityAlreadyAssigned(this, id);
            Id = id;
        }
    }
}
