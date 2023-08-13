using FriendWatch.DTOs.Requests;

namespace FriendWatch.Services.CircleService
{
    public interface ICircleService
    {
        Task CreateCircleAsync(CreateCircleRequest request);
    }
}
