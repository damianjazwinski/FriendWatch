using System.Security.Claims;

namespace FriendWatch.Application.Extensions
{
    public static class ClaimsExtension
    {
        public static int GetUserId(this IEnumerable<Claim> claims)
        {
            return int.Parse(claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        }

        public static string GetUsername(this IEnumerable<Claim> claims)
        {
            return claims.Single(claim => claim.Type == ClaimTypes.Name).Value;
        }
    }
}
