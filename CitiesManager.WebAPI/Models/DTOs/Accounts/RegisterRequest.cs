using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    /// <summary>
    /// Represents a request for user registration.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The person name of the user.
        /// This field is mandatory.
        /// </summary>
        [Required]
        public string PersonName { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the user.
        /// This field is mandatory.    
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The phone number of the user.
        /// Must be between 6 and 16 digits long.
        /// This field is mandatory.
        /// </summary>
        [Required]
        [RegularExpression("^[0-9]{6,16}$", ErrorMessage = "Phone number must be between 6 and 16 digits long.")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// The password for the user account.
        /// This field is mandatory.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The password confirmation.
        /// Must match <see cref="Password"/>.
        /// This field is mandatory.
        /// </summary>
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
