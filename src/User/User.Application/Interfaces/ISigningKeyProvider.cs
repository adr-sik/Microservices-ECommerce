using Microsoft.IdentityModel.Tokens;

namespace User.Application.Interfaces
{
    public interface ISigningKeyProvider
    {
        Task<RsaSecurityKey> GetSigningKeyAsync();
    }
}
