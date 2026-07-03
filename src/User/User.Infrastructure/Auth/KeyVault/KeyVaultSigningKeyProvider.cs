using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using User.Application.Interfaces;

namespace User.Infrastructure.Auth.KeyVault
{
    public sealed class KeyVaultSigningKeyProvider : ISigningKeyProvider
    {
        private readonly IConfiguration _configuration;

        public KeyVaultSigningKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RsaSecurityKey> GetSigningKeyAsync()
        {
            var credential = new DefaultAzureCredential();
            var keyClient = new KeyClient(new Uri(_configuration["KeyVault:Url"]!), credential);

            KeyVaultKey vaultKey = await keyClient.GetKeyAsync("jwt-signing-key");

            var cryptoClient = new CryptographyClient(vaultKey.Id, credential);
            RSAParameters publicParams = vaultKey.Key
                .ToRSA(includePrivateParameters: false)
                .ExportParameters(false);

            var keyVaultRsa = new KeyVaultRsa(cryptoClient, publicParams);

            return new RsaSecurityKey(keyVaultRsa)
            {
                KeyId = vaultKey.Id.ToString()
            };
        }
    }
}
