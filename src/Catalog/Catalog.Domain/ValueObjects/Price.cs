using Catalog.Domain.Exceptions;

namespace Catalog.Domain.ValueObjects
{
    public record Price
    {
        public decimal Value { get; }

        public Price(decimal value)
        {
            if (value <= 0)
                throw new DomainValidationException("Price cannot be negative or 0.", nameof(value));

            Value = Math.Round(value, 2);
        }

        public static implicit operator decimal(Price price) => price.Value;
        public static implicit operator Price(decimal value) => new(value);
    }
}
