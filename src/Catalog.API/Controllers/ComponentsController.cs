using Catalog.Application.Interfaces;
using Catalog.Domain.Entities.ProductComponents;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentsService _componentsService;

        public ComponentsController(IComponentsService componentsService) =>
            _componentsService = componentsService;

        [HttpGet]
        public async Task<List<BaseComponent>> Get() =>
            await _componentsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BaseComponent>> Get(string id)
        {
            var component = await _componentsService.GetAsync(id);
            return component;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaseComponent newComponent)
        {
            await _componentsService.CreateAsync(newComponent);
            return CreatedAtAction(nameof(Get), new { id = newComponent.Id }, newComponent);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] BaseComponent updatedComponent)
        {
            var component = await _componentsService.GetAsync(id);
            updatedComponent.Id = component.Id;
            await _componentsService.UpdateAsync(id, updatedComponent);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var component = await _componentsService.GetAsync(id);
            await _componentsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
