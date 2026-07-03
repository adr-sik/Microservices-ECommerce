using User.Domain.Enums;

namespace User.Domain.Entities
{
    public class UserEntity
    {
        public Guid Id { get; init; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; } = UserRole.User;
        public List<RefreshToken> RefreshTokens { get; private set; } = new();

        private UserEntity() { }

        private UserEntity(Guid id, string username, string email, string passwordHash)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }

        public static UserEntity Create(string username, string email, string passwordHash)
        {
            return new UserEntity(Guid.NewGuid(), username, email, passwordHash);
        }
        public void AddRefreshToken(string tokenHash, DateTime expiry)
        {
            if (RefreshTokens.Count(x => x.IsActive) >= 3)
            {
                var oldest = RefreshTokens
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.CreatedAt)
                    .First();
                oldest.Revoke();
            }

            RefreshTokens.Add(new RefreshToken(tokenHash, expiry, this.Id));
        }

        public void RevokeRefreshToken(string tokenHash)
        {
            var token = RefreshTokens.FirstOrDefault(x => x.TokenHash == tokenHash);
            token?.Revoke();
        }
    }
}
