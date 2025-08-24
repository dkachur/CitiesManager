using Asp.Versioning;
using CitiesManager.Core.DTOs;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Infrastructure.Identity;
using CitiesManager.WebAPI.Models.DTOs.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CitiesManager.WebAPI.Controllers.v1
{
    /// <summary>
    /// Controller responsible for managing user accounts and authorization.
    /// </summary>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">Manager for sign in operations.</param>
        /// <param name="userManager">Manager for user related operations.</param>
        /// <param name="roleManager">Manager for role related operations.</param>
        /// <param name="jwtService">Service for creating JWT token.</param>
        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Registers a new user account and signs in the user upon successful registration.
        /// </summary>
        /// <param name="request">The registration request</param>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing the newly created user if registration succeeds;
        /// otherwise, a <see cref="ProblemDetails"/> object with error information.
        /// </returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> PostRegister(RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return Fail("Given email already registered.");
            
            ApplicationUser user = new()
            {
                Email = request.Email,
                UserName = request.Email,
                PersonName = request.PersonName,
                PhoneNumber = request.Phone
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Fail(errorMessage);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var authRes = await IssueTokensForUser(user);
            return Ok(authRes);
        }

        /// <summary>
        /// Login a user into the account.
        /// </summary>
        /// <param name="request">The request for login.</param>
        /// <returns>The user's name and email.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.Email,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return Fail("Invalid email or password");

            var user = await _userManager.FindByNameAsync(request.Email);
            if (user is null 
                || user.Email is null 
                || user.PersonName is null)
                return NoContent();

            var authRes = await IssueTokensForUser(user);
            return Ok(authRes);
        }

        /// <summary>
        /// Logout from the account.
        /// </summary>
        /// <returns><see cref="NoContentResult"/> object.</returns>
        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        /// <summary>
        /// Refreshes token based on the existing jwt token and refresh token.
        /// </summary>
        /// <param name="tokenRequest">The token request.</param>
        /// <returns>The <see cref="AuthenticationResponse"/> if refreshing was successful;
        /// otherwise, <see cref="ProblemDetails"/> with error message</returns>
        [HttpPost("token")]
        public async Task<ActionResult<AuthenticationResponse>> GenerateNewAccessToken(TokenRequest tokenRequest)
        {
            var principal = _jwtService.GetPrincipalFromJwtToken(tokenRequest.Token);
            if (principal is null) 
                return Fail("Ivalid token");

            var email = principal.FindFirstValue(JwtRegisteredClaimNames.Email);
            if (string.IsNullOrEmpty(email))
                return Fail("Email claims not found");

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Fail("User not found");

            var (isValid, error) = ValidateUser(user);
            if (!isValid) 
                return Fail(error!);

            var authResponse = _jwtService.RefreshJwtToken(new()
            {
                RefreshTokenFromRequest = tokenRequest.RefreshToken,
                RefreshTokenFromUser = user.RefreshToken!, 
                Expiration = user.RefreshTokenExpiration,
                CreateJwtTokenRequest = new()
                {
                    Email = user.Email!, 
                    PersonName = user.PersonName!, 
                    UserId = user.Id,
                }
            });
            if (authResponse is null)
                return Fail("Refresh token is invalid or expired", StatusCodes.Status401Unauthorized);

            await UpdateUserRefreshTokenAsync(user, authResponse); 

            return Ok(authResponse);
        }


        #region Helpers

        private async Task<AuthenticationResponse> IssueTokensForUser(ApplicationUser user)
        {
            var authResponse = _jwtService.CreateJwtToken(new()
            {
                Email = user.Email!,
                PersonName = user.PersonName!,
                UserId = user.Id
            });

            await UpdateUserRefreshTokenAsync(user, authResponse);

            return authResponse;
        }

        private ObjectResult Fail(string message, int status = StatusCodes.Status400BadRequest) 
            => Problem(message, statusCode: status);

        private (bool isValid, string? error) ValidateUser(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                return (false, "User email is missing");

            if (string.IsNullOrWhiteSpace(user.PersonName))
                return (false, "User email is missing");

            if (string.IsNullOrWhiteSpace(user.RefreshToken))
                return (false, "User email is missing");

            return (true, null);
        }

        private async Task UpdateUserRefreshTokenAsync(ApplicationUser user, AuthenticationResponse authResponse)
        {
            user.RefreshToken = authResponse.RefreshToken;
            user.RefreshTokenExpiration = authResponse.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);
        }

        #endregion
    }
}
