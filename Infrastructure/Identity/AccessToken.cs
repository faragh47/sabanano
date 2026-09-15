using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services
{
    public class AccessToken
    {
        public AccessToken(JwtSecurityToken securityToken)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "bearer";
            expires_in = (int) (securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
        }

        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public IList<string> roles { get; set; }
        public long? PersonId { get; set; }
        public ClaimsPrincipal Identity { get; set; }
    }

    public class UserRole
    {
        public string RoleName { get; set; }
    }
}