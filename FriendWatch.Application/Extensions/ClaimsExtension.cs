using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FriendWatch.Application.Extensions
{
    public static class ClaimsExtension
    {
        public static int GetUserId(this IEnumerable<Claim> claims)
        {
            return int.Parse(claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
