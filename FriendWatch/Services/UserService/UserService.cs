using System.Text;

using BCrypt.Net;

using FriendWatch.Data.Repositories.UserRepository;
using FriendWatch.DTOs.Requests;

namespace FriendWatch.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task CreateUserAsync(RegisterRequest userDto)
        {
            var user = new User
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Username = userDto.Username
            };

            await _userRepository.CreateAsync(user);
            return;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user;
        }
    }
}
