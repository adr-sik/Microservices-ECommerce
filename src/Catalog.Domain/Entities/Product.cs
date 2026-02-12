using Catalog.Domain.Constraints;
using Catalog.Domain.Enums;
using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;

namespace Catalog.Domain.Entities
{
    public abstract class Product
    {
        public abstract ProductType Type { get; }
        public string Id { get; init; }
        public string Brand { get; set; }
        public string Model { get; set; }

        private Price _price;
        public decimal Price
        {
            get => _price;
            set => _price = new Price(value);
        }

        public int Stock { get; set; }
        public string? Description { get; set; }

        protected Product(string brand, string model, decimal price, int stock, string? description)
        {
            Brand = brand;
            Model = model;
            Price = price;
            Stock = stock;
            Description = description;
        }
    }
}
