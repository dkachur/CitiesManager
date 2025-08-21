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
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterRequest request)
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
                return Problem(errorMessage);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(user);
        }
    }
}
