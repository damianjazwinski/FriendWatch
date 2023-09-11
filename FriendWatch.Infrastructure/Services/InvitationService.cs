using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;
using FriendWatch.Domain.Common;
using System.Collections.Generic;

namespace FriendWatch.Infrastructure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly ICircleRepository _circleRepository;
        private readonly IUserRepository _userRepository;

        public InvitationService(
            IInvitationRepository invitationRepository, 
            ICircleRepository circleRepository, 
            IUserRepository userRepository)
        {
            _invitationRepository = invitationRepository;
            _circleRepository = circleRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<InvitationDto>> CreateInvitationAsync(InvitationDto dto)
        {
            var existingInvitation = await _invitationRepository.GetByCircleIdAndReceiverIdAsync(dto.CircleId, dto.ReceiverId);

            if (existingInvitation != null)
                return new ServiceResponse<InvitationDto>(false, null, "Invitation for this user already exists");

            var invitation = new Invitation
            {
                Message = dto.Message,
                CircleId = dto.CircleId,
                ReceiverId = dto.ReceiverId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _invitationRepository.CreateAsync(invitation);

            dto.InvitationId = invitation.Id;

            return new ServiceResponse<InvitationDto>(true, dto);
        }

        public async Task<ServiceResponse<List<InvitationDto>>> GetReceivedInvitationAsync(int currentUserId)
        {
            List<Invitation> invitations = await _invitationRepository.GetByReceiverIdAsync(currentUserId);

            if (invitations == null)
                return new ServiceResponse<List<InvitationDto>>(false, null, "Faild to get received invitations");

            var invitationsDto = invitations.Select(invitation => new InvitationDto
            {
                InvitationId = invitation.Id,
                CircleId = invitation.CircleId,
                CreatedAt = invitation.CreatedAt,
                IsAccepted = invitation.IsAccepted,
                InvitationCircleName = invitation.Circle.Name,
                InvitationCircleOwnerUsername = invitation.Circle.Owner.Username,
                InvitationCircleOwnerId = invitation.Circle.OwnerId,
                ReceiverId = invitation.ReceiverId,
            }).ToList();

            return new ServiceResponse<List<InvitationDto>>(true, invitationsDto);
        }

        public async Task<ServiceResponse<List<InvitationDto>>> GetSentInvitationsAsync(int currentUserId)
        {
            List<Invitation> invitations = await _invitationRepository.GetBySenderIdAsync(currentUserId);

            if (invitations == null)
                return new ServiceResponse<List<InvitationDto>>(false, null, "Faild to get sent invitations");

            var invitationsDto = invitations.Select(invitation => new InvitationDto
            {
                InvitationId = invitation.Id,
                CircleId = invitation.CircleId,
                CreatedAt = invitation.CreatedAt,
                IsAccepted = invitation.IsAccepted,
                InvitationCircleName = invitation.Circle.Name,
                ReceiverId = invitation.ReceiverId,
                ReceiverUsername = invitation.Receiver.Username
            }).ToList();

            return new ServiceResponse<List<InvitationDto>>(true, invitationsDto);
        }

        public async Task<ServiceResponse<InvitationDto>> ReplyToInvitationAsync(int invitationId, int currentUserId, bool isAccepted)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);

            if (invitation == null || invitation.ReceiverId != currentUserId)
                return new ServiceResponse<InvitationDto>(false, null, "Failed to get invitation");

            if (!isAccepted)
            {
                // decline invitation
                invitation.UpdatedAt = DateTime.UtcNow;
                invitation.IsAccepted = false;
                await _invitationRepository.UpdateAsync(invitation);
                return new ServiceResponse<InvitationDto>(true, null);
            }

            // accept invitation

            var circle = await _circleRepository.GetByIdAsync(invitation.CircleId);

            if (circle == null)
                return new ServiceResponse<InvitationDto>(false, null, "Faild to get circle");

            var user = await _userRepository.GetByIdAsync(currentUserId);

            if (user == null)
                return new ServiceResponse<InvitationDto>(false, null, "Faild to get user");

            circle.Members.Add(user);

            await _circleRepository.UpdateAsync(circle);
            await _invitationRepository.DeleteAsync(invitation);

            return new ServiceResponse<InvitationDto>(true, null);
        }
    }
}
