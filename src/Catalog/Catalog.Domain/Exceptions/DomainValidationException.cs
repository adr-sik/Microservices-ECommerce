using Catalog.Domain.Constraints;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Exceptions
{
    public class DomainValidationException : CustomDomainException
    {
        public DomainValidationException(string message, string errorCode) : base(message, errorCode) { }
        public DomainValidationException(string message, Exception innerException, string errorCode)
            : base(message, innerException, errorCode) { }

        public static DomainValidationException IdentityAlreadyAssigned(IIdentityConstraint constraint, string id)
            => new($"Cannot assign identity '{id}' to an item that already has one '{constraint.Id}'",
                "IDENTITY_ALREADY_ASSIGNED");

        public static DomainValidationException IncompatibleProduct(IDesignConstraint component, Product product)
            => new($"Component {component.Id} was designed for '{component.DesignedFor}', but was assigned to {product.GetType().Name}",
                "INCOMPATIBLE_PRODUCT");
    }
}
