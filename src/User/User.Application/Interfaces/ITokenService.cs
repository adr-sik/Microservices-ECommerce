using User.Application.DTOs;
using User.Domain.Entities;

namespace User.Application.Interfaces
{
    public interface ITokenService
    {
        public TokenResponse CreateTokenResponse(UserEntity user);
    }
}
