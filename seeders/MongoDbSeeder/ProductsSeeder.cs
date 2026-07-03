using Bogus;
using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Products;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.ProductComponents;
using Catalog.Domain.Entities.ProductTypes;
using Catalog.Domain.Enums;

namespace MongoDbSeeder
{
    public class ProductsSeeder : ISubSeeder
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsSeeder(
            IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task SeedAsync(List<BaseComponent> generatedComponents)
        {
            Console.WriteLine("Seeding products...");

            Console.WriteLine($"Total components received from previous seeder: {generatedComponents.Count}");

            var generatedProducts = new List<Product>();

            var laptopCpus = generatedComponents.Where(c => c is Cpu cpu && cpu.DesignedFor == ProductType.Laptop).ToList();
            var laptopGpus = generatedComponents.Where(c => c is Gpu gpu && gpu.DesignedFor == ProductType.Laptop).ToList();
            var laptopDisplays = generatedComponents.Where(c => c is Display display && display.DesignedFor == ProductType.Laptop).ToList();

            var phoneCpus = generatedComponents.Where(c => c is Cpu cpu && cpu.DesignedFor == ProductType.Phone).ToList();
            var phoneGpus = generatedComponents.Where(c => c is Gpu gpu && gpu.DesignedFor == ProductType.Phone).ToList();
            var phoneDisplays = generatedComponents.Where(c => c is Display display && display.DesignedFor == ProductType.Phone).ToList();
            var phoneCameras = generatedComponents.Where(c => c is Camera camera && camera.DesignedFor == ProductType.Phone).ToList();

            var laptopFaker = new Faker<Laptop>()
                .CustomInstantiator(f =>
                {
                    var brand = f.PickRandom(new[] { "Apple", "Dell", "HP", "Lenovo", "ASUS", "MSI", "Razer" });
                    var model = brand switch
                    {
                        "Apple" => $"MacBook {f.PickRandom("Air", "Pro")} (M{f.Random.Int(1, 3)})",
                        "Dell" => $"XPS {f.Random.Int(13, 17)}",
                        "HP" => $"{f.PickRandom("Spectre x360", "Envy", "Omen")}",
                        "Lenovo" => $"{f.PickRandom("ThinkPad X1", "Legion 5")}",
                        _ => $"{f.Commerce.Color()} Edition Laptop"
                    };

                    return new Laptop(
                        brand: brand,
                        model: model,
                        price: f.Random.Decimal(1000, 5000),
                        description: f.Lorem.Sentence(),
                        cpu: (Cpu)f.PickRandom(laptopCpus),
                        gpu: (Gpu)f.PickRandom(laptopGpus),
                        display: (Display)f.PickRandom(laptopDisplays)
                    );
                })
                .Generate(50);
            generatedProducts.AddRange(laptopFaker);

            // Seed phones
            var phoneFaker = new Faker<Phone>()
                .CustomInstantiator(f =>
                {
                    var brand = f.PickRandom(new[] { "Apple", "Samsung", "Google", "OnePlus", "Xiaomi" });
                    var model = brand switch
                    {
                        "Apple" => $"iPhone {f.PickRandom("13", "13 Pro", "14", "14 Pro", "15", "15 Pro")}",
                        "Samsung" => $"Galaxy {f.PickRandom("S21", "S22", "S23", "Note20", "Note21", "Z Fold3", "Z Flip3")}",
                        "Google" => $"Pixel {f.PickRandom("6", "6a", "7", "7a", "8", "8 Pro")}",
                        "OnePlus" => $"{f.PickRandom("9", "9 Pro", "10", "10 Pro", "11")}",
                        "Xiaomi" => $"{f.PickRandom("Mi 11", "Mi 12", "Redmi Note 10", "Redmi Note 11")}",
                        _ => $"{f.Commerce.Color()} Edition Phone"
                    };

                    return new Phone(
                        brand: brand,
                        model: model,
                        price: f.Random.Decimal(500, 3000),
                        description: f.Lorem.Sentence(),
                        cpu: (Cpu)f.PickRandom(phoneCpus),
                        gpu: (Gpu)f.PickRandom(phoneGpus),
                        display: (Display)f.PickRandom(phoneDisplays),
                        camera: (Camera)f.PickRandom(phoneCameras)
                    );
                })
                .Generate(50);
            generatedProducts.AddRange(phoneFaker);

            var tasks = generatedProducts.Select(p => _productsRepository.CreateAsync(p));
            await Task.WhenAll(tasks);

            Console.WriteLine("Products Seeded.");
        }
    }
}
