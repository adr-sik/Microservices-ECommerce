using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs;
using Ordering.Application.Interfaces;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            var newOrder = await _ordersService.CreateAsync(request, ct);
            return Ok(newOrder);
            //return CreatedAtAction(nameof(Get), new { id = newOrder.Id }, newOrder);
        }
    }
}
