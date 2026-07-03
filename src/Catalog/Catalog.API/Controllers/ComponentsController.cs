using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentsService _componentsService;

        public ComponentsController(IComponentsService componentsService)
        {
            _componentsService = componentsService;
        }

        [HttpGet]
        public async Task<IReadOnlyList<ComponentDto>> Get() =>
            await _componentsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ComponentDto>> Get(string id)
        {
            var component = await _componentsService.GetAsync(id);
            return component;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateComponentRequest request)
        {
            var newComponent = await _componentsService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = newComponent.Id }, newComponent);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateComponentRequest request)
        {
            await _componentsService.UpdateAsync(id, request);
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
