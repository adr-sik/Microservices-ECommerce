namespace Catalog.Domain.Exceptions
{
    public abstract class CustomDomainException : Exception
    {
        public string ErrorCode { get; }
        protected CustomDomainException(string message, string errorCode = "DOMAIN_ERROR")
            : base(message)
        {
            ErrorCode = errorCode;
        }
        protected CustomDomainException(string message, Exception innerException, string errorCode = "DOMAIN_ERROR")
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
