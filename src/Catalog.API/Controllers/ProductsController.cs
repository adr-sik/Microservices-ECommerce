using Catalog.Application.Factories;
using Catalog.Application.Objects;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productsService;
        private readonly ProductFactory _productFactory;
        public ProductsController(ProductsService productsService, ProductFactory productFactory)
        {
            _productsService = productsService;
            _productFactory = productFactory;
        }

        //TODO: Add rest of CRUD operations
        [HttpGet]
        public async Task<List<Product>> Get() =>
            await _productsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _productsService.GetAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductRequest request)
        {
            Product newProduct = await _productFactory.BuildProductAsync(request);

            await _productsService.CreateAsync(newProduct);
            return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product updatedProduct)
        {
            var product = await _productsService.GetAsync(id);
            updatedProduct.Id = product.Id;
            await _productsService.UpdateAsync(id, updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productsService.GetAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            await _productsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
