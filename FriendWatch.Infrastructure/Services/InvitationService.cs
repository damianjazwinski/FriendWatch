using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;

namespace FriendWatch.Infrastructure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;

        public InvitationService(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;    
        }

        public async Task<ServiceResponse<InvitationDto>> CreateInvitationAsync(InvitationDto dto)
        {
            if (dto.ReceiverId != null)
            {
                var existingInvitation = await _invitationRepository.GetByCircleIdAndReceiverIdAsync(dto.CircleId, dto.ReceiverId.Value);

                if (existingInvitation != null)
                    return new ServiceResponse<InvitationDto>(false, null, "Invitation for this user already exists");
            }

            var invitation = new Invitation
            {
                Message = dto.Message,
                CircleId = dto.CircleId,
                ReceiverId = dto.ReceiverId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _invitationRepository.CreateAsync(invitation);

            var createdInvitation = await _invitationRepository.GetByIdAsync(invitation.Id);

            if (createdInvitation == null)
                return new ServiceResponse<InvitationDto>(false, null, "Failed to get invitation");


            var createdInvitationDto = new InvitationDto
            {
                InvitationId = createdInvitation.Id,
                CircleId = createdInvitation.CircleId,
                Message = createdInvitation.Message,
                ReceiverId = createdInvitation.ReceiverId,
                InvitationCircleName = createdInvitation.Circle.Name,
                InvitationCircleOwnerId = createdInvitation.Circle.Owner.Id,
                InvitationCircleOwnerUsername = createdInvitation.Circle.Owner.Username
            };

            return new ServiceResponse<InvitationDto>(true, createdInvitationDto);
        }
    }
}
