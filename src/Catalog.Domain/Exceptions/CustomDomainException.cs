using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Exceptions
{
    public class CustomDomainException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }
        public CustomDomainException() : base() { }
        public CustomDomainException(string message, string errorCode = "DOMAIN_ERROR", int statusCode = 400)
            : base(message) 
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
        public CustomDomainException(string message, Exception innerException, string errorCode = "DOMAIN_ERROR")
            : base(message, innerException) 
        {
            ErrorCode = errorCode;
        }
    }
}
