using Microsoft.AspNetCore.Identity;
using User.Application.Interfaces;
using User.Domain.Entities;

namespace User.Infrastructure.Auth
{
    public class IdentityPasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<UserEntity> _hasher = new();
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var result = _hasher.VerifyHashedPassword(null!, hash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
