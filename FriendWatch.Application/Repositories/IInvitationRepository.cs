using FriendWatch.Domain.Entities;

namespace FriendWatch.Application.Repositories
{
    public interface IInvitationRepository
    {
        public Task CreateAsync(Invitation invitation);
    }
}
