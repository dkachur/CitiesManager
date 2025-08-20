using CitiesManager.Infrastructure.Identity;
using CitiesManager.WebAPI.Models.DTOs.Account;
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

        public async Task<ActionResult<bool>> IsEmailAlreadyRegistered(string email)
        {
            if (await _userManager.FindByEmailAsync(email) == null)
                return Ok(false);
            
            return Ok(true);
        }

        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }

            ApplicationUser user = new()
            {
                Email = request.Email,
                UserName = request.Name,
                PersonName = request.Name,
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
