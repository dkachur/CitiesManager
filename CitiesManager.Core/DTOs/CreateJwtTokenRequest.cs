namespace CitiesManager.Core.DTOs
{
    public class CreateJwtTokenRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PersonName { get; set; } = string.Empty;
    }
}
