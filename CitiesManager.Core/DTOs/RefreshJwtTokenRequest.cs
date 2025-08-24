namespace CitiesManager.Core.DTOs
{
    public class RefreshJwtTokenRequest
    {
        public required string RefreshTokenFromRequest { get; init; }
        public required string RefreshTokenFromUser { get; init; }
        public required DateTime Expiration { get; init; }
        public required CreateJwtTokenRequest CreateJwtTokenRequest { get; init; }
    }
}