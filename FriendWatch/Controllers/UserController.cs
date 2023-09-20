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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("avatar")]
        [Authorize]
        public async Task<IActionResult> SetUserAvatar([FromForm] SetUserAvatarRequest request)
        {
            if (request.UserAvatarImage == null)
                return BadRequest(new ErrorResponse("Failed to set avatar"));

            if (request.UserAvatarImage.Length == 0)
                return BadRequest(new ErrorResponse("Image file is empty"));

            if (request.UserAvatarImage.Length > 5242880)
                return BadRequest(new ErrorResponse("Image file is larger than 5MB"));

            var userId = HttpContext.User.Claims.GetUserId();
            var userDto = new UserDto() { Id = userId };

            using var memoryStream = new MemoryStream();
            await request.UserAvatarImage.CopyToAsync(memoryStream);

            userDto.AvatarImageDto = new ImageFileDto
            {
                FileName = request.UserAvatarImage.FileName,
                ContentType = request.UserAvatarImage.ContentType,
                Data = memoryStream.ToArray(),
            };

            var result = await _userService.SetUserAvatar(userDto);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            if (result.Data?.UserAvatarUrl == null)
                return BadRequest(new ErrorResponse("Failed to set avatar"));

            return Ok(new SetUserAvatarResponse { UserAvatarUrl = result.Data.UserAvatarUrl });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.User.Claims.GetUserId();
            var result = await _userService.GetByIdAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse(result.Message!));

            var userResponse = new GetUserResponse { Id = result.Data!.Id, Username = result.Data.Username, UserAvatarUrl = result.Data.UserAvatarUrl };
            return Ok(userResponse);
        }
    }
}
