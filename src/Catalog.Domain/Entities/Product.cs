using Catalog.Domain.Constraints;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities
{
    public abstract class Product : IIdentityConstraint
    {
        public string? Id { get; protected set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
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

        public void SetIdentity(string id)
        {
            if(!string.IsNullOrEmpty(Id))
                throw DomainValidationException.IdentityAlreadyAssigned(this, id);
            Id = id;
        }
    }
}
