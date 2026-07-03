using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using User.Application.DTOs;
using User.Application.Interfaces;
using User.Application.Util;
using User.Domain.Entities;

namespace User.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly RsaSecurityKey _signingKey;

        public const int AccessTokenExpirationMinutes = 15;
        public const int RefreshTokenExpirationDays = 7;

        public TokenService(IConfiguration configuration, RsaSecurityKey signingKey)
        {
            _configuration = configuration;
            _signingKey = signingKey;
        }

        public TokenResponse CreateTokenResponse(UserEntity user)
        {
            var refreshToken = CreateRefreshToken(user);
            var refreshTokenHash = TokenHasher.Hash(refreshToken);
            var refreshTokenExpires = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays);

            user.AddRefreshToken(refreshTokenHash, refreshTokenExpires);

            return new TokenResponse(
                CreateAccessToken(user),
                refreshToken,
                DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes),
                refreshTokenExpires);
        }

        private string CreateAccessToken(UserEntity user)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Email] = user.Email,
                ["username"] = user.Username,
                ["role"] = user.Role.ToString()
            };

            var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Claims = claims,
                Expires = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes),
                SigningCredentials = credentials
            };

            return tokenHandler.CreateToken(descriptor);
        }

        private string CreateRefreshToken(UserEntity user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
