using CitiesManager.Core.DTOs;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _options;

        public JwtService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public AuthenticationResponse CreateJwtToken(CreateJwtTokenRequest request)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

            var claims = new Claim[] {
               new(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),   // Subject (user ID)
               new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),   // JWT unique ID
               new(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),                               // Issued at (date and time of token generation)
               new(JwtRegisteredClaimNames.Email, request.Email),             // Email of the user
               new(JwtRegisteredClaimNames.Name, request.PersonName),         // Name of the user
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenGenerator = new JwtSecurityToken(
                _options.Issuer, 
                _options.Audience, 
                claims, 
                expires: expiration,
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            string token = tokenHandler.WriteToken(tokenGenerator);

            return new()
            {
                Email = request.Email,
                PersonName = request.PersonName,
                Expire = expiration,
                Token = token
            };
        }
    }
}
