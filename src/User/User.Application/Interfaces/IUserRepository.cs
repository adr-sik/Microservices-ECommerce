

using User.Domain.Entities;

namespace User.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(UserEntity user, CancellationToken ct = default);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<UserEntity?> GetByIdAsync(Guid id);
        Task<UserEntity?> GetByUsernameAsync(string username);
        Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash);
        Task UpdateAsync(UserEntity user);
        Task AddRefreshTokenAsync(Guid userId, RefreshToken token);
    }
}
