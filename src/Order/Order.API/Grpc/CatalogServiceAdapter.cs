using Catalog.API.Protos;
using Ordering.Application.Interfaces;
using ApplicationProduct = Ordering.Application.DTOs.Product;


namespace Ordering.API.Grpc
{
    public class CatalogServiceAdapter : ICatalogService
    {
        private readonly ProductService.ProductServiceClient _client;
        public CatalogServiceAdapter(ProductService.ProductServiceClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ApplicationProduct>> GetProductsAsync(List<string> ids)
        {
            var request = new GetProductsRequest();
            request.Ids.AddRange(ids);
            var response = await _client.GetProductsAsync(request);
            return response.Products.Select(p => new ApplicationProduct
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = FromUnit64(p.Price)
            });
        }

        private static decimal FromUnit64(ulong value)
        {
            return value / 100.00m;
        }
    }
}
