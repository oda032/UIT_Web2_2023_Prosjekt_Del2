using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.Server.Helper
{
    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
                return null;

            return jsonToken.Claims;
        }
    }
}
