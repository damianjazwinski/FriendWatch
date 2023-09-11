using FriendWatch.Application.DTOs;
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
        [HttpPost("create/link")]
        public async Task<IActionResult> CreateInvitationLink(GetCircleInvitationLinkRequest request)
        {
            var invitationDtoRequest = new InvitationDto
            {
                Message = request.Message,
                CircleId = request.CircleId
            };

            var serviceResponse = await _invitationService.CreateInvitationAsync(invitationDtoRequest);

            if (!serviceResponse.IsSuccess)
                return BadRequest(new ErrorResponse(serviceResponse.Message!));

            var invitationDtoResponse = serviceResponse.Data;

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = Uri.UriSchemeHttps;
            uriBuilder.Path = "invitation/link";
            uriBuilder.Host = "localhost";
            uriBuilder.Query = $"id={invitationDtoResponse!.InvitationId}";

            var invitationLink = uriBuilder.Uri.ToString();

            return Ok(new { Link = invitationLink});
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateInvitation(CircleInvitationRequest request)
        {
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
    }
}
