namespace CitiesManager.WebAPI.Models.DTOs.Accounts
{
    /// <summary>
    /// Represents a response containing a new JWT access token and refresh token pair.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// The newly generated JWT access token for authorization.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// The newly issued refresh token that should be stored and used for future access token requests.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
