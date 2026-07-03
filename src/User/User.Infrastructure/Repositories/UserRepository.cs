using Microsoft.EntityFrameworkCore;
using User.Application.Interfaces;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserEntity user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserEntity?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash)
        {
            return await _context.Set<RefreshToken>().FirstOrDefaultAsync(t => t.TokenHash == hash);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UserEntity user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddRefreshTokenAsync(Guid userId, RefreshToken token)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            user.AddRefreshToken(token.TokenHash, token.ExpiresAt);
            await _context.SaveChangesAsync();
        }
    }
}
