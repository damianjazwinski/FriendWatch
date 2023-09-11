using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IInvitationRepository
    {
        public Task CreateAsync(Invitation invitation);
        public Task<Invitation?> GetByIdAsync(int id);
        public Task<Invitation?> GetByCircleIdAndReceiverIdAsync(int circleId, int receiverId);
    }
}
