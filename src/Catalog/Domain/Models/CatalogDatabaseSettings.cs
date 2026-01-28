namespace Catalog.Domain.Models
{
    public class CatalogDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string ComponentsCollectionName { get; set; } = null!;
    }
}
