using CitiesManager.Core.DTOs;

namespace CitiesManager.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(CreateJwtTokenRequest request);
    }
}
