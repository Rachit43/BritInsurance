using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Infrastructure.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BritInsurance.Infrastructure.Identity
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IdentityConfig _config;

        public TokenProvider(IOptions<IdentityConfig> config)
        {
            _config = config.Value;
        }

        public UserIdentityDto GenerateJwtToken(string user, string[] roles)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.AccessExpirationMinutes),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            return new UserIdentityDto
            {
                UserName = user,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
