namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    /// <summary>
    /// Represents a response returned after a successful user registration.
    /// Contains user details such as person name, email and phone number.
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        /// The email address of the registered user.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// The person name of the registered user.
        /// </summary>
        public string PersonName { get; set; } = string.Empty;
        /// <summary>
        /// The phone number of the registered user.
        /// </summary>
        public string Phone { get; set; } = string.Empty;
    }
}
