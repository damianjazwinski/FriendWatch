using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IWatchRepository
    {
        Task CreateAsync(Watch watch);
        Task<List<Watch>> GetByUserIdAsync(int currentUserId);
    }
}
