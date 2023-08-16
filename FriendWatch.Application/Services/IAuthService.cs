using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<(string, string)>> Login(UserDto userDto);
        Task<ServiceResponse<(string, string)>> RefreshToken(string token);
    }
}
