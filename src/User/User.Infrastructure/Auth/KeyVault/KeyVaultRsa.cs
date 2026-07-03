using Azure.Security.KeyVault.Keys.Cryptography;
using System.Security.Cryptography;

namespace User.Infrastructure.Auth.KeyVault
{
    public sealed class KeyVaultRsa : RSA
    {
        private readonly CryptographyClient _client;
        private readonly RSAParameters _publicParams;

        public KeyVaultRsa(CryptographyClient client, RSAParameters publicParams)
        {
            _client = client;
            _publicParams = publicParams;
        }

        public override byte[] SignHash(
            byte[] hash,
            HashAlgorithmName hashAlgorithm,
            RSASignaturePadding padding)
        {
            var algorithm = hashAlgorithm.Name switch
            {
                "SHA256" => Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256,
                "SHA384" => Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384,
                "SHA512" => Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512,
                _ => throw new NotSupportedException(
                        $"Hash algorithm {hashAlgorithm.Name} is not supported.")
            };

            var result = _client.Sign(algorithm, hash);
            return result.Signature;
        }

        public override bool VerifyHash(
            byte[] hash,
            byte[] signature,
            HashAlgorithmName hashAlgorithm,
            RSASignaturePadding padding)
        {
            var algorithm = hashAlgorithm.Name switch
            {
                "SHA256" => Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS256,
                "SHA384" => Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS384,
                "SHA512" => Azure.Security.KeyVault.Keys.Cryptography.SignatureAlgorithm.RS512,
                _ => throw new NotSupportedException(
                        $"Hash algorithm {hashAlgorithm.Name} is not supported.")
            };

            var result = _client.Verify(algorithm, hash, signature);
            return result.IsValid;
        }

        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            if (includePrivateParameters)
                throw new CryptographicException(
                    "Private key material does not leave Key Vault.");
            return _publicParams;
        }

        public override void ImportParameters(RSAParameters parameters)
            => throw new NotSupportedException(
                "Key material cannot be imported into a Key Vault-backed RSA key.");
    }
}
