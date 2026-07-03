using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Pagination;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.DTOs.Sorting;
using Catalog.Application.Interfaces;
using Catalog.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<PaginatedResponse<ProductDto>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ProductSortColumn? sortBy = null,
            [FromQuery] ProductSortDirection? sortDirection = null)
        {
            var pagination = new PaginationRequest(page, pageSize);
            ProductSortRequest? sort = null;
            sort = GetSortDefaults(sortBy, sortDirection);

            return await _productsService.GetAsync(pagination, sort);
        }

        [HttpPost("search")]
        public async Task<PaginatedResponse<ProductDto>> Get(
            [FromBody] ProductFilter filter,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ProductSortColumn? sortBy = null,
            [FromQuery] ProductSortDirection? sortDirection = null)
        {
            var pagination = new PaginationRequest(page, pageSize);
            ProductSortRequest? sort = null;
            sort = GetSortDefaults(sortBy, sortDirection);

            return await _productsService.GetAsync(filter, pagination, sort);
        }

        [HttpGet("{type}")]
        public async Task<PaginatedResponse<ProductDto>> GetByType(
            string type,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ProductSortColumn? sortBy = null,
            [FromQuery] ProductSortDirection? sortDirection = null)
        {
            Enum.TryParse<ProductType>(type, true, out var productType);
            var pagination = new PaginationRequest(page, pageSize);
            ProductSortRequest? sort = null;
            sort = GetSortDefaults(sortBy, sortDirection);

            return await _productsService.GetByTypeAsync(productType, pagination, sort);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProductDto>> Get(string id)
        {
            var product = await _productsService.GetAsync(id);
            return product;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var newProduct = await _productsService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(
            string id,
            [FromBody] CreateProductRequest updatedProduct)
        {
            await _productsService.UpdateAsync(id, updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productsService.GetAsync(id);
            await _productsService.RemoveAsync(id);
            return NoContent();
        }

        // Private methods
        private static ProductSortRequest? GetSortDefaults(
            ProductSortColumn? sortBy,
            ProductSortDirection? sortDirection)
        {
            if (sortBy.HasValue || sortDirection.HasValue)
            {
                return new ProductSortRequest(
                    sortBy ?? ProductSortColumn.Price,
                    sortDirection ?? ProductSortDirection.Ascending
                );
            }
            return null;
        }
    }
}
