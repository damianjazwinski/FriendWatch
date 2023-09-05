using System.Security.Claims;

using FriendWatch.Application.Services;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)!.Value;
            var result = await _userService.GetByIdAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse { Messages = new string[] { result.Message } });

            var userResponse = new GetUserResponse { Id = result.Data!.Id, Username = result.Data.Username };
            return Ok(userResponse);
        }
    }
}
