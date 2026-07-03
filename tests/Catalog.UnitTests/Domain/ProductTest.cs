using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;
using Catalog.Domain.Exceptions;

namespace Catalog.UnitTests.Domain
{
    public class ProductTest
    {
        private static Cpu CreateValidCpu() => new Cpu
        {
            DesignedFor = ProductType.Laptop,
            Brand = CpuBrand.Intel,
            Model = "Test CPU",
            NumberOfCores = 8
        };

        private static Gpu CreateValidGpu() => new Gpu
        {
            DesignedFor = ProductType.Laptop,
            Brand = GpuBrand.Nvidia,
            Model = "Test GPU",
            VRAM = 6
        };

        private static Display CreateValidDisplay() => new Display
        {
            DesignedFor = ProductType.Laptop,
            ScreenSizeInches = 15.6m,
            Resolution = "1080p",
            RefreshRateHz = 60
        };

        [Theory]
        [MemberData(nameof(GetInvalidLaptopComponents))]
        public void Constructor_ShouldThrowValidationException_WhenAnyComponentIsMismatched(Cpu cpu, Gpu gpu, Display display)
        {
            // Act & Assert
            Assert.Throws<DomainValidationException>(() =>
                new Laptop(
                    brand: "TestBrand",
                    model: "TestModel",
                    price: 1000m,
                    description: "Test Description",
                    cpu: cpu,
                    gpu: gpu,
                    display: display
                ));
        }

        public static IEnumerable<object[]> GetInvalidLaptopComponents()
        {
            // Case 1: Bad CPU
            var badCpu = CreateValidCpu();
            badCpu.DesignedFor = ProductType.Phone;
            yield return new object[] { badCpu, CreateValidGpu(), CreateValidDisplay() };

            // Case 2: Bad GPU
            var badGpu = CreateValidGpu();
            badGpu.DesignedFor = ProductType.Phone;
            yield return new object[] { CreateValidCpu(), badGpu, CreateValidDisplay() };

            // Case 3: Bad Display
            var badDisplay = CreateValidDisplay();
            badDisplay.DesignedFor = ProductType.Phone;
            yield return new object[] { CreateValidCpu(), CreateValidGpu(), badDisplay };
        }

        [Fact]
        public void Constructor_ShouldCreateLaptop_WhenComponentsAreDesignedForLaptop()
        {
            // Arrange

            var cpu = CreateValidCpu();
            var gpu = CreateValidGpu();
            var display = CreateValidDisplay();

            // Act

            Laptop laptop = new Laptop(
                    brand: "TestBrand",
                    model: "TestModel",
                    price: 1000m,
                    description: "Test Description",
                    cpu: cpu,
                    gpu: gpu,
                    display: display
                    );

            // Assert

            Assert.NotNull(laptop);
            Assert.Equal("TestBrand", laptop.Brand);
            Assert.Equal("TestModel", laptop.Model);
            Assert.Equal(1000m, laptop.Price);
            Assert.Equal("Test Description", laptop.Description);
            Assert.Equal(cpu, laptop.Cpu);
            Assert.Equal(gpu, laptop.Gpu);
            Assert.Equal(display, laptop.Display);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Constructor_ShouldThrow_WhenPriceIsZeroOrLess(decimal invalidPrice)
        {
            // Arrange

            var cpu = CreateValidCpu();
            var gpu = CreateValidGpu();
            var display = CreateValidDisplay();

            // Act & Assert
            Assert.Throws<DomainValidationException>(() =>
                new Laptop("Brand", "Model", invalidPrice, "Desc", cpu, gpu, display));
        }
    }
}
