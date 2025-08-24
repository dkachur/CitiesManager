using CitiesManager.Core.DTOs;
using System.Security.Claims;

namespace CitiesManager.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(CreateJwtTokenRequest request);
        AuthenticationResponse? RefreshJwtToken(RefreshJwtTokenRequest request);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string token);
    }
}
