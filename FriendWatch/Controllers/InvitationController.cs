using FriendWatch.Application.DTOs;
using FriendWatch.Application.Extensions;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly IUserService _userService;

        public InvitationController(IInvitationService invitationService, IUserService userService)
        {
            _invitationService = invitationService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateInvitation(CircleInvitationRequest request)
        {
            if (request.ReceiverUsername == HttpContext.User.Claims.GetUsername())
                return BadRequest(new ErrorResponse("You can't invite yourself to circle"));

            var getUserResponse = await _userService.GetByUsernameAsync(request.ReceiverUsername);

            if (!getUserResponse.IsSuccess)
                return BadRequest(new ErrorResponse(getUserResponse.Message!));

            var receiver = getUserResponse.Data;

            var invitationDtoRequest = new InvitationDto
            {
                CircleId = request.CircleId,
                ReceiverId = receiver!.Id,
            };

            var createInvitationResponse = await _invitationService.CreateInvitationAsync(invitationDtoRequest);

            if (!createInvitationResponse.IsSuccess)
                return BadRequest(new ErrorResponse(createInvitationResponse.Message!));

            return Ok(new SuccessResponse());
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetInvitations()
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var getSentInvitationsResponse = await _invitationService.GetSentInvitationsAsync(currentUserId);

            if (!getSentInvitationsResponse.IsSuccess)
                return BadRequest(new ErrorResponse(getSentInvitationsResponse.Message!));

            var getReceivedInvitationsResponse = await _invitationService.GetReceivedInvitationAsync(currentUserId);

            if (!getReceivedInvitationsResponse.IsSuccess)
                return BadRequest(new ErrorResponse(getReceivedInvitationsResponse.Message!));

            return Ok(new GetUsersInvitationsResponse
            {
                SentInvitations = getSentInvitationsResponse.Data!,
                ReceivedInvitations = getReceivedInvitationsResponse.Data!
            });
        }

        [Authorize]
        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToInvitation(ReplyToInvitationRequest request)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();
            var replyToInvitationResponse = await _invitationService.ReplyToInvitationAsync(request.InvitationId, currentUserId, request.Acceptance);

            if (!replyToInvitationResponse.IsSuccess)
                return BadRequest(new ErrorResponse(replyToInvitationResponse.Message!));

            return Ok(new SuccessResponse());
        }
    }
}
