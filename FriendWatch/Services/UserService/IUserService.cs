using FriendWatch.DTOs.Requests;

namespace FriendWatch.Services.UserService
{
    public interface IUserService
    {
        Task CreateUserAsync(RegisterRequest userDto);
    }
}
