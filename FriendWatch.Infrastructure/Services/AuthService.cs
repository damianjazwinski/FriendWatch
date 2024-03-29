﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using FriendWatch.Application.DTOs;
using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FriendWatch.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<(string, string)>> Login(UserRequestDto userDto)
        {
            var user = await _userRepository.GetByUsernameAsync(userDto.Username);

            const string errorMessage = "Username or password incorrect";

            if (user is null)
            {
                return new ServiceResponse<(string, string)>(false, message: errorMessage);
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return new ServiceResponse<(string, string)>(false, message: errorMessage);
            }

            return new ServiceResponse<(string, string)>(true, GenerateJSONWebToken(user));
        }

        public async Task<ServiceResponse<(string, string)>> RefreshToken(string token)
        {
            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var username = decodedToken.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return new ServiceResponse<(string, string)>(false);
            }

            var user = await _userRepository.GetByUsernameAsync(username!);

            if (user is null)
            {
                return new ServiceResponse<(string, string)>(false);
            }

            return new ServiceResponse<(string, string)>(true, GenerateJSONWebToken(user));
        }

        private (string, string) GenerateJSONWebToken(User user)
        {
            var accessClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("tokenType", "access")
            };
            var refreshClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("tokenType", "refresh")
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            _ = int.TryParse(_configuration.GetSection("Cookies").GetSection("AccessTokenCookieExpirationMinutes").Value, out int minutes);
            _ = int.TryParse(_configuration.GetSection("Cookies").GetSection("RefreshTokenCookieExpirationDays").Value, out int days);

            var token = new JwtSecurityToken(claims: accessClaims, expires: DateTime.Now.AddMinutes(minutes), signingCredentials: creds);
            var refreshToken = new JwtSecurityToken(claims: refreshClaims, expires: DateTime.Now.AddDays(days), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            return (jwt, refreshJwt);

        }
    }
}
