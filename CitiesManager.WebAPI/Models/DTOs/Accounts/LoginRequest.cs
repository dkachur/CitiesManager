using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    /// <summary>
    /// Represents a request for user login. 
    /// </summary>
    public class LoginRequest()
    {
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The password for user account.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}