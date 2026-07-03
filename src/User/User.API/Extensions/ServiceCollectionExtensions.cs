using Microsoft.IdentityModel.Tokens;
using User.Application.Interfaces;
using User.Application.Services;
using User.Infrastructure.Auth.KeyVault;
using User.Infrastructure.Auth.Local;

namespace User.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public async static Task<RsaSecurityKey> AddAuthServices(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
        {
            ISigningKeyProvider signingKeyProvider = env.IsDevelopment()
                ? new LocalSigningKeyProvider(config)
                : new KeyVaultSigningKeyProvider(config);

            var signingKey = await signingKeyProvider.GetSigningKeyAsync();

            services.AddSingleton(signingKey);
            services.AddSingleton<ITokenService, TokenService>();

            return signingKey;
        }
    }
}
