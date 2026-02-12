using Catalog.Domain.Exceptions;

namespace Catalog.Domain.ValueObjects
{
    internal record Price
    {
        internal decimal Value { get; private set; }

        internal Price(decimal value)
        {
            if (value < 0)
                throw new DomainValidationException("Price cannot be negative.", nameof(value));

            Value = Math.Round(value, 2);
        }

        public static implicit operator decimal(Price price) => price.Value;
        public static implicit operator Price(decimal value) => new(value);
    }
}
