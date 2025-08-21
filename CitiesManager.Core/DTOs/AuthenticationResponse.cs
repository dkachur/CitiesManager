namespace CitiesManager.Core.DTOs
{
    public class AuthenticationResponse
    {
        public string Token { get; set; } = string.Empty;
        public string PersonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Expire { get; set; }
    }
}
