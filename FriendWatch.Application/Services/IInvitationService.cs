using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface IInvitationService
    {
        Task<ServiceResponse<InvitationDto>> CreateInvitationAsync(InvitationDto invitationDto);
        Task<ServiceResponse<List<InvitationDto>>> GetSentInvitationsAsync(int currentUserId);
        Task<ServiceResponse<List<InvitationDto>>> GetReceivedInvitationAsync(int currentUserId);
        Task<ServiceResponse<InvitationDto>> ReplyToInvitationAsync(int invitationId, int currentUserId, bool isAccepted);
    }
}
