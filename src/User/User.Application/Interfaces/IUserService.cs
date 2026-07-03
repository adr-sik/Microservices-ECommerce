using User.Application.DTOs;

namespace User.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterUserRequest request);
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
