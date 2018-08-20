namespace PictIt.Areas.User.Controllers
{
    using System.Threading.Tasks;
    using System.Web;

    using IdentityServer4.Events;
    using IdentityServer4.Services;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;
    using PictIt.Models;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [Area("User")]
    public class LoginController : AnonymousApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginController> logger, IIdentityServerInteractionService interaction, IEventService events)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _interaction = interaction;
            _events = events;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser userToLogin)
        {
            var context = await _interaction.GetAuthorizationContextAsync(userToLogin.ReturnUrl);
            if (context == null) return StatusCode(209, "/home");

            var user = await _userManager.FindByEmailAsync(userToLogin.Email);
            if (user == null)
            {
                _logger.LogError($"There is no existing user using {userToLogin.Email} as an email address.");

                await _events.RaiseAsync(new UserLoginFailureEvent(userToLogin.Email, "wrong email/password combination"));
                return UnprocessableEntity("wrong email/password");
            }

            // lockoutOnFailure = increment lockout count in case of failure
            var result = await _signInManager.PasswordSignInAsync(user, userToLogin.Password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation($"{userToLogin.Email} successfully loged in.");

                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.FirstName + " " + user.LastName));
                if (_interaction.IsValidReturnUrl(userToLogin.ReturnUrl) || Url.IsLocalUrl(userToLogin.ReturnUrl))
                {
                    return StatusCode(209, userToLogin.ReturnUrl);
                }

                return StatusCode(209, "/home");
            }

            if (result.RequiresTwoFactor)
            {
                return StatusCode(209, $"/login-2fa?returnUrl={ HttpUtility.UrlEncode(userToLogin.ReturnUrl) }");
            }

            if (result.IsLockedOut)
            {
                _logger.LogError($"{userToLogin.Email} has its account locked out.");

                await _events.RaiseAsync(new UserLoginFailureEvent(userToLogin.Email, "account locked out"));

                return StatusCode(209, $"/lockout?email={userToLogin.Email}");
            }
            
            _logger.LogError($"{userToLogin.Email} didn't provide its correct password.");

            await _events.RaiseAsync(new UserLoginFailureEvent(userToLogin.Email, "wrong email/password combination"));

            return UnprocessableEntity("wrong email/password");
        }
    }
}
