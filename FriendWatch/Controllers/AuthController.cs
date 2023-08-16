using System.Net.Http.Headers;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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

        [HttpGet("public-ping")]
        public async Task<IActionResult> PublicPing()
        {
            return Ok("public-ping");
        }        
        
        [Authorize]
        [HttpGet("forbidden-ping")]
        public async Task<IActionResult> ForbiddenPing()
        {
            return Ok("forbidden-ping");
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // TODO: Add confirmation validation logic

            var userDto = new UserDto
            {
                Username = request.Username,
                Password = request.Password
            };

            await _userService.CreateUserAsync(userDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var userDto = new UserDto
            {
                Username = request.Username,
                Password = request.Password
            };

            var loginResult = await _authService.Login(userDto);

            if (!loginResult.IsSuccess)
                return BadRequest("Username or password incorrect");

            var response = new LoginResponse
            {
                AccessToken = loginResult.Data.Item1,
                RefreshToken = loginResult.Data.Item2
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Headers[HeaderNames.Authorization].FirstOrDefault()?.Split()[1];

            if (token == null)
                return BadRequest("Missing refresh token");

            var refreshResult = await _authService.RefreshToken(token!);

            if (!refreshResult.IsSuccess)
                return BadRequest("Invalid refresh token");

            var response = new LoginResponse
            {
                AccessToken = refreshResult.Data.Item1,
                RefreshToken = refreshResult.Data.Item2
            };
            return Ok(response);
        }
    }
}
