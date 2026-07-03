using User.Application.DTOs;
using User.Application.Interfaces;
using User.Domain.Entities;

namespace User.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public Task<bool> IsEmailUniqueAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(RegisterUserRequest request)
        {
            var hash = _passwordHasher.HashPassword(request.Password);

            var user = UserEntity.Create(request.Name, request.Email, hash);

            await _userRepository.AddAsync(user);
        }
    }
}
