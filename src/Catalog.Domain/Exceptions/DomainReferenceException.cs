using Catalog.Domain.Constraints;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Exceptions
{
    public class DomainReferenceException : Exception
    {
        public string? ErrorCode { get; }
        private DomainReferenceException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public static DomainReferenceException NotFound(string id, string itemType)
            => new DomainReferenceException($"{itemType} with Id: {id} was not found.", "ID_NOTFOUND");

        public static DomainReferenceException TypeMismatch(string id, string expectedType, string actualType)
            => new DomainReferenceException($"Item with Id: {id} is a '{actualType}' but was requested as a '{expectedType}'.", "TYPE_MISMATCH");
    }
}

