using FriendWatch.Application.DTOs;

namespace FriendWatch.Application.Services
{
    public interface ICircleService
    {
        Task CreateCircleAsync(CircleDto circleDto, int currentUserId);
    }
}
