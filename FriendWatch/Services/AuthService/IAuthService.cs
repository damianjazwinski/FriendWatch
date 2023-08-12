using FriendWatch.DTOs.Requests;

namespace FriendWatch.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<(string, string)>> Login(LoginRequest request);
        Task<ServiceResponse<(string, string)>> RefreshToken(string token);
    }
}
