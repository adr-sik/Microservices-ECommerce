using Microsoft.AspNetCore.Mvc;
using User.Application.DTOs;
using User.Application.Interfaces;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var tokenResponse = await _authService.LoginAsync(request.Username, request.Password);
            SetAuthCookies(tokenResponse);
            return Ok(new { Message = "Logged in" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies["refresh_token"] == null)
                return BadRequest(new { Message = "No refresh token found" });

            await _authService.LogoutAsync(Request.Cookies["refresh_token"]);
            Response.Cookies.Delete("access_token", AccessTokenCookieSettings);
            Response.Cookies.Delete("refresh_token", RefreshTokenCookieSettings);
            Response.Cookies.Delete("XSRF-TOKEN", CsrfTokenCookieSettings);
            return Ok(new { Message = "Logged out" });
        }

        [HttpPost("refresh-tokens")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var tokenResponse = await _authService.RefreshTokenAsync(refreshToken);
            SetAuthCookies(tokenResponse);
            return Ok(new { Message = "Token refreshed" });
        }

        // Private methods
        private void SetAuthCookies(TokenResponse tokenResponse)
        {
            var csrfToken = Guid.NewGuid().ToString();
            Response.Cookies.Append("access_token", tokenResponse.AccessToken, AccessTokenCookieSettings);
            Response.Cookies.Append("refresh_token", tokenResponse.RefreshToken, RefreshTokenCookieSettings);
            Response.Cookies.Append("XSRF-TOKEN", csrfToken, CsrfTokenCookieSettings);
        }

        private CookieOptions AccessTokenCookieSettings => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            MaxAge = TimeSpan.FromMinutes(15)
        };

        private CookieOptions RefreshTokenCookieSettings => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            MaxAge = TimeSpan.FromDays(7)
        };

        private CookieOptions CsrfTokenCookieSettings => new()
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Lax
        };
    }
}
