using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    /// <summary>
    /// Represents a request to refresh a JWT access token.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// The existing (possibly expired) JWT access token.
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// The refresh token used to generate a new access token.
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
