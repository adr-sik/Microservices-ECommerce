using Catalog.Domain.Constraints;

namespace Catalog.Domain.Exceptions
{
    public class DomainReferenceException : CustomDomainException
    {
        public DomainReferenceException(string message, string errorCode) : base(message, errorCode) { }
        public DomainReferenceException(string message, Exception innerException, string errorCode)
            : base(message, innerException, errorCode) { }

        public static DomainReferenceException NotFound(IIdentityConstraint id, string itemType)
            => new($"{itemType} with Id: {id} was not found.",
                "IDENTITY_NOT_FOUND");

        public static DomainReferenceException TypeMismatch(IIdentityConstraint id, string expectedType, string actualType)
            => new($"Item with Id: {id} is a '{actualType}' but was requested as a '{expectedType}'.",
                "TYPE_MISMATCH");
    }
}

