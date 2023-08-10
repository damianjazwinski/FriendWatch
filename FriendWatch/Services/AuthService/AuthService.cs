using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using FriendWatch.Data.Repositories.UserRepository;
using FriendWatch.DTOs.Requests;
using System.Text;

namespace FriendWatch.Services.AuthService
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

        public async Task<ServiceResponse<(string, string)>> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByUsername(request.Username);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
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
                new Claim("tokenType", "access")
            };
            var refreshClaims = new List<Claim>
            {
                new Claim("tokenType", "refresh")
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: accessClaims, expires: DateTime.Now.AddMinutes(20), signingCredentials: creds);
            var refreshToken = new JwtSecurityToken(claims: refreshClaims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            return (jwt, refreshJwt);

        }
    }
}
