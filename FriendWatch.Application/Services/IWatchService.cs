using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface IWatchService
    {
        Task<ServiceResponse<WatchDto>> AddCommentToWatch(CommentDto commentDto);
        Task<ServiceResponse<WatchDto>> CreateWatchAsync(WatchDto watchDto);
        Task<ServiceResponse<List<WatchDto>>> GetAllWatchesForUserAsync(int currentUserId);
    }
}
