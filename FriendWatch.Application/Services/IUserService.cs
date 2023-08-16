using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDto userDto);
        Task<ServiceResponse<UserDto>> GetByIdAsync(int id);
    }
}
