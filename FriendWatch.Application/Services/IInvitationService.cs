using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Application.Services
{
    public interface IInvitationService
    {
        Task<ServiceResponse<InvitationDto>> CreateInvitationAsync(InvitationDto invitationDto);
    }
}
