using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;

namespace Catalog.UnitTests.Domain
{
    public class PriceTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-0.01)]
        public void Constructor_ShouldThrow_WhenValueIsZeroOrLess(decimal invalidValue)
        {
            Assert.Throws<DomainValidationException>(() => new Price(invalidValue));
        }

        [Theory]
        [InlineData(10.555, 10.56)]
        [InlineData(10.554, 10.55)]
        [InlineData(9.995, 10.00)]
        [InlineData(9.991, 9.99)]
        [InlineData(10, 10.00)]
        public void Price_ShouldAlwaysRoundToTwoDecimals_RegardlessOfEntry(decimal input, decimal expected)
        {
            Price priceFromOperator = input;

            Price priceFromConstructor = new Price(input);

            Assert.Equal(expected, (decimal)priceFromOperator);
            Assert.Equal(expected, (decimal)priceFromConstructor);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToDecimal()
        {
            Price price = new Price(19.99m);
            decimal decimalValue = price;
            Assert.Equal(19.99m, decimalValue);
        }
        [Fact]
        public void ImplicitConversion_ShouldConvertFromDecimal()
        {
            decimal decimalValue = 29.99m;
            Price price = decimalValue;
            Assert.Equal(29.99m, (decimal)price);
        }
    }
}
