using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface ICircleService
    {
        Task<ServiceResponse<CircleDto>> CreateCircleAsync(CircleDto circleDto, int currentUserId);

        Task<ServiceResponse<List<CircleDto>>> GetOwnedCirclesAsync(int currentUserId);
        Task<ServiceResponse<List<CircleDto>>> GetJoinedCirclesAsync(int currentUserId);
    }
}
