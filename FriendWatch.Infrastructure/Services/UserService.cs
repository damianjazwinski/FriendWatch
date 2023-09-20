using FriendWatch.Application.DTOs;
using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;

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
                    UserAvatarUrl = user.Avatar != null ? $"/api/download/{user.Avatar.FileName}" : null
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

        public async Task<ServiceResponse<UserDto>> SetUserAvatar(UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(userDto.Id);

            if (user == null)
                return new ServiceResponse<UserDto>(false, null, "User not found");

            if (userDto.AvatarImageDto == null || userDto.AvatarImageDto.Data == null)
                return new ServiceResponse<UserDto>(false, null, "Failed to setting avatar");

            var fileExtension = Path.GetExtension(userDto.AvatarImageDto.FileName);
            var directoryInfo = Directory.CreateDirectory(@"files");
            var generatedFileName = $"{Path.ChangeExtension(Path.GetRandomFileName(), fileExtension)}";
            var fullPathWithName = Path.Combine(directoryInfo.ToString(), generatedFileName);

            userDto.AvatarImageDto.Path = fullPathWithName;
            userDto.AvatarImageDto.FileName = generatedFileName;

            using (var stream = File.Create(fullPathWithName))
            {
                await stream.WriteAsync(userDto.AvatarImageDto.Data, 0, userDto.AvatarImageDto.Data.Length);
            }

            user.Avatar = new ImageFile
            {
                FileName = generatedFileName,
                ContentType = userDto.AvatarImageDto.ContentType!,
                Path = Path.Combine("files", generatedFileName)
            };


            await _userRepository.UpdateAsync(user);

            userDto.UserAvatarUrl = $"/api/download/{user.Avatar.FileName}";

            return new ServiceResponse<UserDto>(true, userDto);
        }
    }
}
