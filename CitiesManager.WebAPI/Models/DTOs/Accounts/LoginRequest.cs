using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginRequest()
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}