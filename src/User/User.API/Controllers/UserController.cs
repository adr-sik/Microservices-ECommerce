using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.DTOs;
using User.Application.Interfaces;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                await _userService.RegisterAsync(request);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new { Message = "You are authenticated!" });
        }

        [HttpGet("test-role")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestRole()
        {
            return Ok(new { Message = "You have the Admin role!" });
        }

        [HttpGet("debug-claims")]
        [Authorize]
        public IActionResult Debug()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
