﻿using FriendWatch.Application.DTOs;
using FriendWatch.Application.Services;
using FriendWatch.DTOs.Requests;
using FriendWatch.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string AccessTokenCookieName = "AccessToken";
        private const string RefreshTokenCookieName = "RefreshToken";

        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IAuthService authService, IConfiguration configuration)
        {
            _userService = userService;
            _authService = authService;
            _configuration = configuration;
        }

        [Authorize(Policy = "RefreshPolicy")]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete(AccessTokenCookieName);
            HttpContext.Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions { Path = "api/auth" });
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var userDto = new UserRequestDto
            {
                Username = request.Username,
                Password = request.Password
            };

            var result = await _userService.CreateUserAsync(userDto);

            if (!result.IsSuccess)
                return BadRequest(new ErrorResponse { Messages = new string[] { result.Message } });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var userDto = new UserRequestDto
            {
                Username = request.Username,
                Password = request.Password
            };

            var loginResult = await _authService.Login(userDto);

            if (!loginResult.IsSuccess)
                return BadRequest(new ErrorResponse(loginResult.Message!));



            var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(_configuration.GetValue<int>("Cookies:AccessTokenCookieExpirationMinutes"));
            var refreshTokenExpiration = DateTimeOffset.UtcNow.AddDays(_configuration.GetValue<int>("Cookies:RefreshTokenCookieExpirationDays"));

            var accessCookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = true,
                Expires = accessTokenExpiration,
                IsEssential = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            };

            var refreshCookieOptions = new CookieOptions()
            {
                Path = "/api/auth",
                HttpOnly = true,
                Expires = refreshTokenExpiration,
                IsEssential = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            };

            HttpContext.Response.Cookies.Append(AccessTokenCookieName, loginResult.Data.Item1, accessCookieOptions);
            HttpContext.Response.Cookies.Append(RefreshTokenCookieName, loginResult.Data.Item2, refreshCookieOptions);

            var response = new AuthResponse
            {
                AccessTokenExpiration = accessTokenExpiration.ToUnixTimeMilliseconds(),
                RefreshTokenExpiration = refreshTokenExpiration.ToUnixTimeMilliseconds()
            };

            return Ok(response);
        }

        [Authorize(Policy = "RefreshPolicy")]
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = HttpContext.Request.Cookies["RefreshToken"];

            if (token == null)
                return BadRequest("Missing refresh token");

            var refreshResult = await _authService.RefreshToken(token!);

            if (!refreshResult.IsSuccess)
                return BadRequest("Invalid refresh token");

            var accessTokenExpiration = DateTimeOffset.UtcNow.AddMinutes(_configuration.GetValue<int>("Cookies:AccessTokenCookieExpirationMinutes"));
            var refreshTokenExpiration = DateTimeOffset.UtcNow.AddDays(_configuration.GetValue<int>("Cookies:RefreshTokenCookieExpirationDays"));

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
                Path = "/api/auth",
                HttpOnly = true,
                Expires = refreshTokenExpiration,
                IsEssential = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
            };

            HttpContext.Response.Cookies.Append(AccessTokenCookieName, refreshResult.Data.Item1, accessCookieOptions);
            HttpContext.Response.Cookies.Append(RefreshTokenCookieName, refreshResult.Data.Item2, refreshCookieOptions);

            HttpContext.Response.Cookies.Append(AccessTokenCookieName, refreshResult.Data.Item1, accessCookieOptions);
            HttpContext.Response.Cookies.Append(RefreshTokenCookieName, refreshResult.Data.Item2, refreshCookieOptions);

            var response = new AuthResponse
            {
                AccessTokenExpiration = accessTokenExpiration.ToUnixTimeMilliseconds(),
                RefreshTokenExpiration = refreshTokenExpiration.ToUnixTimeMilliseconds()
            };

            return Ok(response);
        }
    }
}
