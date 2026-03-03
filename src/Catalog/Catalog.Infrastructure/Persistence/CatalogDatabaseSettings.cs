namespace Catalog.Infrastructure.Persistence
{
    public class CatalogDatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string ProductsCollectionName { get; set; }
        public required string ComponentsCollectionName { get; set; }
    }
}
