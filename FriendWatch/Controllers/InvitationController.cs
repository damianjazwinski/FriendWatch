using FriendWatch.Application.DTOs;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;

using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CircleInvitationRequest request)
        {
            var invitationDto = new InvitationDto
            {
                Message = request.Message,
                UserId = request.UserId,
                CircleId = request.CircleId
            };

            await _invitationService.SendInvitationAsync(invitationDto);

            return Ok();
        }
    }
}
