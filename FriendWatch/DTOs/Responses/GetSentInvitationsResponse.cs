using FriendWatch.Application.DTOs;

namespace FriendWatch.DTOs.Responses
{
    public record GetUsersInvitationsResponse
    {
        public List<InvitationDto> SentInvitations { get; set; } = new List<InvitationDto>();
        public List<InvitationDto> ReceivedInvitations { get; set; } = new List<InvitationDto>();
    }
}
