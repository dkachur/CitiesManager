using CitiesManager.Infrastructure.Identity;
using CitiesManager.WebAPI.Models.DTOs.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace CitiesManager.WebAPI.Controllers.v1
{
    /// <summary>
    /// Controller responsible for managing user accounts and authorization.
    /// </summary>
    public class AccountController : CustomControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">Manager for sign in operations.</param>
        /// <param name="userManager">Manager for user related operations.</param>
        /// <param name="roleManager">Manager for role related operations.</param>
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Checks whether the specified email address is available for registration.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns><c>true</c> if the email is available; otherwise, <c>false</c>.</returns>
        public async Task<ActionResult<bool>> IsEmailAvailable(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
                return Ok(true);

            return Ok(false);
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
        public async Task<ActionResult<RegisterResponse>> PostRegister(RegisterRequest request)
        {
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
                return Problem(errorMessage, statusCode: StatusCodes.Status400BadRequest);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new RegisterResponse()
            {
                Email = user.Email,
                PersonName = user.PersonName,
                Phone = user.PhoneNumber
            });
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
                return Problem("Invalid email or password");

            var user = await _userManager.FindByNameAsync(request.Email);
            if (user is null)
                return NoContent();

            return Ok(new { personName = user.PersonName, email = user.Email });
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
    }
}
