namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    public class RegisterResponse
    {
        public string Email { get; set; } = string.Empty;
        public string PersonName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
