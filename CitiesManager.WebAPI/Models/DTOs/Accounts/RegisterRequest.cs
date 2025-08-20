using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs.Account
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Remote("IsEmailAlreadyRegistered", "Account", ErrorMessage = "Given email already registered.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[0-9]{6,16}$", ErrorMessage = "Phone number must be between 6 and 16 characters long.")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
