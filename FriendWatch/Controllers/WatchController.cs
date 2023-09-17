using FriendWatch.Application.DTOs;
using FriendWatch.Application.Extensions;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : ControllerBase
    {
        private readonly IWatchService _watchService;

        public WatchController(IWatchService watchService)
        {
            _watchService = watchService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateWatchRequest request)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();

            var watchDto = new WatchDto
            {
                CircleId = request.CircleId,
                CreatorId = currentUserId,
                Message = request.Message,
                ExternalLink = request.ExternalLink,
            };

            var result = await _watchService.CreateWatchAsync(watchDto);

            if(!result.IsSuccess)
                return BadRequest(new ErrorResponse("Failed to create watch"));

            return Ok(new SuccessResponse());
        }
    }
}
