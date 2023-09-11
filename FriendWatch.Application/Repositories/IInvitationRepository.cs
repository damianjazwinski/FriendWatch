using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IInvitationRepository
    {
        Task CreateAsync(Invitation invitation);
        Task<Invitation?> GetByIdAsync(int id);
        Task<Invitation?> GetByCircleIdAndReceiverIdAsync(int circleId, int receiverId);
        Task<List<Invitation>> GetBySenderIdAsync(int currentUserId);
        Task<List<Invitation>> GetByReceiverIdAsync(int currentUserId);
        Task UpdateAsync(Invitation invitation);
        Task DeleteAsync(Invitation invitation);
    }
}
