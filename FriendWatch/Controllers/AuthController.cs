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



            var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(20);
            var refreshTokenExpiration = DateTimeOffset.UtcNow.AddDays(1);

            var accessCookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = true,
                Expires = accessTokenExpiration,
                IsEssential = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
            };

            var refreshCookieOptions = new CookieOptions()
            {
                Path = "api/auth/refresh",
                HttpOnly = true,
                Expires = refreshTokenExpiration,
                IsEssential = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
            };

            HttpContext.Response.Cookies.Append("AccessToken", loginResult.Data.Item1, accessCookieOptions);
            HttpContext.Response.Cookies.Append("RefreshToken", loginResult.Data.Item2, refreshCookieOptions);

            var response = new LoginResponse
            {
                AccessTokenExpiration = accessTokenExpiration.ToUnixTimeMilliseconds(),
                RefreshTokenExpiration = refreshTokenExpiration.ToUnixTimeMilliseconds()
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Headers[HeaderNames.Authorization].FirstOrDefault()?.Split()[1];

            if (token == null)
                return BadRequest("Missing refresh token");

            var refreshResult = await _authService.RefreshToken(token!);

            if (!refreshResult.IsSuccess)
                return BadRequest("Invalid refresh token");

            // TODO: Remove cookies and add new ones

            return Ok();
        }
    }
}
