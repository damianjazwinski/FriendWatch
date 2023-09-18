using System.Text;

using BCrypt.Net;

using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<UserDto>> CreateUserAsync(UserRequestDto userDto)
        {
            // check for already existing user with given username
            var existingUser = await _userRepository.GetByUsernameAsync(userDto.Username);
            
            if (existingUser != null)
                return new ServiceResponse<UserDto>(false, null, "Username is already taken");

            await _userRepository.CreateAsync(new User
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Username = userDto.Username
            });

            return new ServiceResponse<UserDto>(true, null);
        }

        public async Task<ServiceResponse<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                };

                return new ServiceResponse<UserDto>(true, userDto);
            }

            return new ServiceResponse<UserDto>(false, null, "User not found");
        }

        public Task<ServiceResponse<UserDto>> GetByIdAsync(string id)
        {
            var userId = int.Parse(id);
            return GetByIdAsync(userId);
        }

        public async Task<ServiceResponse<UserDto>> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                };

                return new ServiceResponse<UserDto>(true, userDto);
            }

            return new ServiceResponse<UserDto>(false, null, "User not found");
        }
    }
}
