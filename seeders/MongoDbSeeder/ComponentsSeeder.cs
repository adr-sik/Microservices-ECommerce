using Bogus;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Enums;
using MongoDB.Driver;

namespace MongoDbSeeder
{
    public class ComponentsSeeder : ISubSeeder
    {
        private readonly IComponentsRepository _componentsRepository;

        public ComponentsSeeder(IComponentsRepository componentsRepository)
        {
            _componentsRepository = componentsRepository;
        }

        public async Task SeedAsync(List<BaseComponent> generatedComponents)
        {
            Console.WriteLine("Seeding Components...");

            var computerCpuBrands = new CpuBrand[] { CpuBrand.Intel, CpuBrand.AMD, CpuBrand.Apple };
            var mobileCpuBrands = new CpuBrand[] { CpuBrand.Apple, CpuBrand.Qualcomm, CpuBrand.MediaTek, CpuBrand.Google };
            var computerGpuBrands = new GpuBrand[] { GpuBrand.Nvidia, GpuBrand.AMD, GpuBrand.Intel };
            var mobileGpuBrands = new GpuBrand[] { GpuBrand.Adreno, GpuBrand.Apple, GpuBrand.Mali, GpuBrand.Xclipse };

            // 1. Seed Computer CPUs
            var computerCpuFaker = new Faker<Cpu>()
                .CustomInstantiator(f =>
                {
                    var brand = f.PickRandom(computerCpuBrands);
                    var model = brand switch
                    {
                        CpuBrand.Intel => $"Intel Core i{f.Random.Int(3, 9)}-{f.Random.Int(3000, 13000)}",
                        CpuBrand.AMD => $"AMD Ryzen {f.Random.Int(3, 9)} {f.Random.Int(3000, 9000)}X",
                        CpuBrand.Apple => $"Apple M{f.Random.Int(1, 3)}",
                        _ => "Unknown"
                    };

                    return new Cpu { Brand = brand, Model = model, NumberOfCores = f.Random.Int(2, 16), DesignedFor = ProductType.Laptop };
                })
                .Generate(10);
            Console.WriteLine($"Generated {computerCpuFaker.Count} computer CPU entities.");
            generatedComponents.AddRange(computerCpuFaker);

            // 2. Seed Mobile CPUs
            var mobileCpuFaker = new Faker<Cpu>()
                .CustomInstantiator(f =>
                {
                    var brand = f.PickRandom(mobileCpuBrands);
                    var model = brand switch
                    {
                        CpuBrand.Apple => $"Apple A{f.Random.Int(10, 16)} Bionic",
                        CpuBrand.Qualcomm => $"Snapdragon {f.Random.Int(400, 8_000)}",
                        CpuBrand.MediaTek => $"MediaTek Dimensity {f.Random.Int(500, 1_200)}",
                        CpuBrand.Google => $"Google Tensor {f.Random.Int(1, 3)}",
                        _ => "Unknown"
                    };

                    return new Cpu { Brand = brand, Model = model, NumberOfCores = f.Random.Int(2, 8), DesignedFor = ProductType.Phone };
                })
                .Generate(10);
            Console.WriteLine($"Generated {mobileCpuFaker.Count} mobile CPU entities.");
            generatedComponents.AddRange(mobileCpuFaker);

            // 3. Seed Computer GPUs
            var computerGpuFaker = new Faker<Gpu>()
                .CustomInstantiator(f =>
                {
                    var brand = f.PickRandom(computerGpuBrands);
                    var model = brand switch
                    {
                        GpuBrand.Nvidia => $"NVIDIA GeForce RTX {f.Random.Int(3050, 4090)}",
                        GpuBrand.AMD => $"AMD Radeon RX {f.Random.Int(5500, 7900)}",
                        GpuBrand.Intel => $"Intel Iris Xe {f.Random.Int(1, 96)}",
                        _ => "Unknown"
                    };

                    return new Gpu { Brand = brand, Model = model, VRAM = f.Random.Int(2, 24), DesignedFor = ProductType.Laptop };
                })
                .Generate(10);
            Console.WriteLine($"Generated {computerGpuFaker.Count} computer GPU entities.");
            generatedComponents.AddRange(computerGpuFaker);

            // 4. Seed Mobile GPUs
            var mobileGpuFaker = new Faker<Gpu>()
                .CustomInstantiator(f =>
                {
                    var brand = f.PickRandom(mobileGpuBrands);
                    var model = brand switch
                    {
                        GpuBrand.Adreno => $"Adreno {f.Random.Int(300, 700)}",
                        GpuBrand.Apple => $"Apple GPU {f.Random.Int(4, 16)}-core",
                        GpuBrand.Mali => $"Mali-G{f.Random.Int(57, 78)}",
                        GpuBrand.Xclipse => $"Xclipse {f.Random.Int(920, 980)}",
                        _ => "Unknown"
                    };

                    return new Gpu { Brand = brand, Model = model, VRAM = f.Random.Int(2, 12), DesignedFor = ProductType.Phone };
                })
                .Generate(10);
            Console.WriteLine($"Generated {mobileGpuFaker.Count} mobile GPU entities.");
            generatedComponents.AddRange(mobileGpuFaker);

            // 5. Seed Laptop Displays
            var computerDisplayFaker = new Faker<Display>()
                .CustomInstantiator(f => new Display
                {
                    DesignedFor = ProductType.Laptop,
                    ScreenSizeInches = f.Random.Decimal(12.0m, 18.0m),
                    Resolution = f.PickRandom(new[] { "1600×900", "1920x1080", "3840x2160" }),
                    RefreshRateHz = f.PickRandom(new[] { 60, 120, 144, 240 })
                })
                .Generate(10);
            Console.WriteLine($"Generated {computerDisplayFaker.Count} computer display entities.");
            generatedComponents.AddRange(computerDisplayFaker);

            // 6. Seed Mobile Displays
            var mobileDisplayFaker = new Faker<Display>()
                .CustomInstantiator(f => new Display
                {
                    DesignedFor = ProductType.Phone,
                    ScreenSizeInches = f.Random.Decimal(4.0m, 7.0m),
                    Resolution = f.PickRandom(new[] { "1280x720", "1920x1080", "2560x1440" }),
                    RefreshRateHz = f.PickRandom(new[] { 60, 90, 120, 144 })
                })
                .Generate(10);
            Console.WriteLine($"Generated {mobileDisplayFaker.Count} mobile display entities.");
            generatedComponents.AddRange(mobileDisplayFaker);

            // 7. Seed Mobile Cameras
            var mobileCameraFaker = new Faker<Camera>()
                .CustomInstantiator(f => new Camera
                {
                    DesignedFor = ProductType.Phone,
                    Megapixels = f.Random.Int(8, 108)
                })
                .Generate(10);
            Console.WriteLine($"Generated {mobileCameraFaker.Count} mobile camera entities.");
            generatedComponents.AddRange(mobileCameraFaker);

            foreach (var component in generatedComponents)
            {
                await _componentsRepository.CreateAsync(component);
            }

            Console.WriteLine($"Seeded {generatedComponents.Count} components directly to DB.");
            Console.WriteLine("Components Seeded.");
        }
    }
}
