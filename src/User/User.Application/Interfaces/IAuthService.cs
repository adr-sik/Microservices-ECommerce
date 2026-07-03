using User.Application.DTOs;

namespace User.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse?> LoginAsync(string username, string password);
        Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
