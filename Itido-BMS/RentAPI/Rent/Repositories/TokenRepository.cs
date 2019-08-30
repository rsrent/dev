using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly SymmetricSecurityKey _key;
        public TokenRepository(IOptions<SymmetricKey> options)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        }

        public string GenerateToken(string userID)
        {
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, userID),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.UtcNow.AddDays(30)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}")
            };

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
                    ),
                new JwtPayload(claims));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
