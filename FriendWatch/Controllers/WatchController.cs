using FriendWatch.Application.DTOs;
using FriendWatch.Application.Extensions;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;
using FriendWatch.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FriendWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : ControllerBase
    {
        private readonly IWatchService _watchService;
        private readonly IHubContext<CommentsHub> _commentsHubContext;

        public WatchController(IWatchService watchService, IHubContext<CommentsHub> commentsHubContext)
        {
            _watchService = watchService;
            _commentsHubContext = commentsHubContext;
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
                ExpirationDate = request.ExpirationDate,
            };

            if (request.ExpirationDate != null && request.ExpirationDate < DateTime.UtcNow)
                return BadRequest(new ErrorResponse("Expiration date from past"));

            var result = await _watchService.CreateWatchAsync(watchDto);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse("Failed to create watch"));

            return Ok(new SuccessResponse());
        }

        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> CommentWatch(CommentWatchRequest request)
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();

            var commentDto = new CommentDto
            {
                CommenterId = currentUserId,
                Content = request.Content,
                WatchId = request.WatchId,
            };

            var result = await _watchService.AddCommentToWatch(commentDto);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            await _commentsHubContext.Clients.All.SendAsync("NewCommentEvent", "Test payload");
            return Ok(new SuccessResponse());
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var currentUserId = HttpContext.User.Claims.GetUserId();

            var result = await _watchService.GetAllWatchesForUserAsync(currentUserId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));


            return Ok(new GetUserWatchesResponse
            {
                Watches = result.Data!.Select(watch => new WatchResponseDto
                {
                    WatchId = watch.Id!.Value,
                    CircleId = watch.CircleId,
                    CircleName = watch.Circle!.Name,
                    Message = watch.Message,
                    ExternalLink = watch.ExternalLink,
                    ExpirationDate = watch.ExpirationDate,
                    CreatorId = watch.CreatorId,
                    CreatorName = watch.Creator!.Username,
                    CreatorAvatarUrl = watch.Creator.UserAvatarUrl,
                    CreatedAt = watch.CreatedAt!.Value,
                    UpdatedAt = watch.UpdatedAt,
                    Comments = watch.Comments,
                }).ToList()
            });
        }
    }
}

