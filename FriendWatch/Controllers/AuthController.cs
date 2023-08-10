using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;
using FriendWatch.Services.AuthService;
using FriendWatch.Services.UserService;

using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _userService.CreateUserAsync(request);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var loginResult = await _authService.Login(request);

            if (!loginResult.IsSuccess)
                return BadRequest("Username or password incorrect");

            var response = new LoginResponse
            {
                AccessToken = loginResult.Data.Item1,
                RefreshToken = loginResult.Data.Item2
            };

            return Ok(response);
        }
    }
}
