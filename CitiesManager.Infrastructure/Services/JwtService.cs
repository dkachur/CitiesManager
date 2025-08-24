using CitiesManager.Core.DTOs;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CitiesManager.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;

        public JwtService(IOptions<JwtOptions> jwtOptions, IOptions<RefreshTokenOptions> refreshTokenOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _refreshTokenOptions = refreshTokenOptions.Value;
        }

        public AuthenticationResponse CreateJwtToken(CreateJwtTokenRequest request)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);

            var claims = new Claim[] {
               new(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),   // Subject (user ID)
               new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),   // JWT unique ID
               new(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),                               // Issued at (date and time of token generation)
               new(JwtRegisteredClaimNames.Email, request.Email),             // Email of the user
               new(JwtRegisteredClaimNames.Name, request.PersonName),         // Name of the user
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenGenerator = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: expiration,
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            string token = tokenHandler.WriteToken(tokenGenerator);

            return new()
            {
                Email = request.Email,
                PersonName = request.PersonName,
                Expiration = expiration,
                Token = token,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_refreshTokenOptions.ExpirationMinutes)
            };
        }

        public AuthenticationResponse? RefreshJwtToken(RefreshJwtTokenRequest request)
        {
            if (!IsValidRefreshRequest(request))
                return null;

            return CreateJwtToken(request.CreateJwtTokenRequest);
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),

                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal claimsPrincipal;

            try
            { 
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken); 
            }
            catch (Exception)
            {
                return null;
            }

            if (securityToken is not JwtSecurityToken jwtToken
                || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return claimsPrincipal;
        }

        private string GenerateRefreshToken()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(bytes);
        }

        private bool IsValidRefreshRequest(RefreshJwtTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshTokenFromRequest)
                || string.IsNullOrWhiteSpace(request.RefreshTokenFromUser)
                || request.RefreshTokenFromRequest != request.RefreshTokenFromUser
                || request.Expiration <= DateTime.UtcNow)
                return false;

            return true;
        }
    }
}
