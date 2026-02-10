using Catalog.Domain.Constraints;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Exceptions
{
    public class DomainValidationException : CustomDomainException
    {
        public DomainValidationException() { }
        public DomainValidationException(string message) : base(message) { }
        public DomainValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        public static DomainValidationException IdentityAlreadyAssigned(IIdentityConstraint constraint, string id)
            => new($"Cannot assign identity '{id}' to an item that already has one '{constraint.Id}'");
        public static DomainValidationException IncompatibleProduct(IDesignConstraint component, Product product)
            => new($"Component {component.Id} was designed for '{component.DesignedFor}', but was assigned to {product.GetType().Name}");
    }
}
