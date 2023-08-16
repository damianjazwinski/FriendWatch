using FriendWatch.Application.DTOs;

namespace FriendWatch.Application.Services
{
    public interface IInvitationService
    {
        Task SendInvitationAsync(InvitationDto request);
    }
}
