using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.IdentityModel.Tokens;


using FriendWatch.Application.Services;
using FriendWatch.Application.Repositories;
using FriendWatch.Domain.Common;
using FriendWatch.Domain.Entities;
using FriendWatch.Application.DTOs;
using Microsoft.Extensions.Configuration;

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

        public async Task<ServiceResponse<(string, string)>> Login(UserDto userDto)
        {
            var user = await _userRepository.GetByUsernameAsync(userDto.Username);

            if (user is null)
            {
                return new ServiceResponse<(string, string)>(false);
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return new ServiceResponse<(string, string)>(false);
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
