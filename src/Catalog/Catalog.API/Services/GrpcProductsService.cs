using Catalog.API.Protos;
using Catalog.Application.Interfaces;
using Grpc.Core;

namespace Catalog.API.Services
{
    public class GrpcProductsService : ProductService.ProductServiceBase
    {
        private readonly IProductsService _productsService;

        public GrpcProductsService(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public override async Task<GetProductsResponse> GetProducts(
            GetProductsRequest request,
            ServerCallContext context)
        {
            var products = await _productsService.GetAllByIdAsync(request.Ids.ToList());

            var response = new GetProductsResponse();
            response.Products.AddRange(products.Select(p => new Product
            {
                ProductId = p.Id,
                ProductName = p.Model,
                Price = ToUnit64(p.Price)
            }));

            return response;
        }

        private static ulong ToUnit64(decimal value)
        {
            return (ulong)Math.Round(value * 100, 0, MidpointRounding.AwayFromZero);
        }
    }
}
