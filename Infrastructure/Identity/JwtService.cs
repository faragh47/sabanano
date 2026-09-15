using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Application.Common.Interfaces;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSetting;
        private readonly ICurrentUserService _currentUserService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<ApplicationUser> signInManager, ICurrentUserService currentUserService)
        {
            _siteSetting = settings.Value;
            this.signInManager = signInManager;
            _currentUserService = currentUserService;
        }

        public async Task<AccessToken> GenerateAsync(ApplicationUser user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(encryptionkey), 
                SecurityAlgorithms.Aes128KW, 
                SecurityAlgorithms.Aes128CbcHmacSha256
                );

            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
               // NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
               // Expires = DateTime.Now.AddYears(1),// ( _siteSetting.JwtSettings.ExpirationMinutes),
                Expires = DateTime.Now.AddMinutes( _siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };
            

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            //string encryptedJwt = tokenHandler.WriteToken(securityToken);

            return new  AccessToken(securityToken);
        }

        public int? ValidateToken(string token)        {
            if (token == null)
                return null;
            var secretKey = Encoding.UTF8.GetBytes((string)"KsJOpVMI2GS81uGAC7SX0LFBjODKXjs4NILYoh1T97hCs2wi8cHvXOhRIASq");
            var encryptionKey = Encoding.UTF8.GetBytes((string)"4JrMk2px5DORVrVd");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("KsJOpVMI2GS81uGAC7SX0LFBjODKXjs4NILYoh1T97hCs2wi8cHvXOhRIASq");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true, //default : false
                    ValidAudience = "SabaNano",
                    ValidateIssuer = true, //default : false
                    ValidIssuer = "SabaNano",

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);
                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        private List<UserRole> ConvertClaimsToRoleList(IEnumerable<Claim> claims)
        {
            var rolesList = new List<UserRole>();
            foreach(Claim claim in claims)
            {
                rolesList.Add(new UserRole
                {
                    RoleName = claim.Value
                });
            }

            return rolesList;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(ApplicationUser user)
        {
          //  var result = await signInManager.ClaimsFactory.CreateAsync(user);
            //add custom claims
           // var list = new List<Claim>(result.Claims);
            //list.Add(new Claim(ClaimTypes.MobilePhone, "09123456987"));

            //JwtRegisteredClaimNames.Sub
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.MobilePhone, "09123456987"),
                new Claim(securityStampClaimType, user.SecurityStamp.ToString())
            };

            //var roles = new Role[] { new Role { Name = "Admin" } };
            //foreach (var role in roles)
            //    list.Add(new Claim(ClaimTypes.Role, role.Name));

            return list;
        }
    }
}
