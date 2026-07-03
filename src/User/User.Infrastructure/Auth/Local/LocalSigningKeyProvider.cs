using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using User.Application.Interfaces;

namespace User.Infrastructure.Auth.Local
{
    public sealed class LocalSigningKeyProvider : ISigningKeyProvider
    {
        private readonly IConfiguration _configuration;

        public LocalSigningKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<RsaSecurityKey> GetSigningKeyAsync()
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(_configuration["Jwt:RsaPrivateKey"]!);
            return Task.FromResult(new RsaSecurityKey(rsa));
        }
    }
}
