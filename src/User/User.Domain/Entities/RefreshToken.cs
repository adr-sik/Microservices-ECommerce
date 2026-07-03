namespace User.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; init; }
        public string TokenHash { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool Revoked { get; private set; }
        public Guid UserId { get; private set; }
        public UserEntity User { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !Revoked && !IsExpired;

        private RefreshToken() { }

        public RefreshToken(string tokenHash, DateTime expiresAt, Guid userId)
        {
            Id = Guid.NewGuid();
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            Revoked = false;
            UserId = userId;
        }

        public void Revoke()
        {
            Revoked = true;
        }
    }
}
