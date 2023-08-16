using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;

namespace FriendWatch.Infrastructure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;

        public InvitationService(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;    
        }

        public async Task SendInvitationAsync(InvitationDto dto)
        {
            var invitation = new Invitation
            {
                Message = dto.Message,
                CircleId = dto.CircleId,
                ReceiverId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _invitationRepository.CreateAsync(invitation);
        }
    }
}
