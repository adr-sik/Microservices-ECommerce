using User.Application.DTOs;
using User.Application.Interfaces;
using User.Application.Util;

namespace User.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)

        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            var tokenResponse = _tokenService.CreateTokenResponse(user);
            await _userRepository.UpdateAsync(user);

            return tokenResponse;
        }

        public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
        {
            var tokenHash = TokenHasher.Hash(refreshToken);
            var tokenRecord = await _userRepository.GetRefreshTokenByHashAsync(tokenHash);

            if (tokenRecord == null || !tokenRecord.IsActive)
            {
                throw new UnauthorizedAccessException("Session invalid or expired.");
            }

            var user = await _userRepository.GetByIdAsync(tokenRecord.UserId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var tokenResponse = _tokenService.CreateTokenResponse(user);
            await _userRepository.UpdateAsync(user);

            return tokenResponse;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenHash = TokenHasher.Hash(refreshToken);
            var tokenRecord = await _userRepository.GetRefreshTokenByHashAsync(tokenHash);
            if (tokenRecord != null && tokenRecord.IsActive)
            {
                var user = await _userRepository.GetByIdAsync(tokenRecord.UserId);
                if (user != null)
                {
                    user.RevokeRefreshToken(tokenHash);
                    await _userRepository.UpdateAsync(user);
                }
            }
        }
    }
}
