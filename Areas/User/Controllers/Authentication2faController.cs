namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;
    using PictIt.Models;

    [Area("User")]
    public class Authentication2faController : AuthorizedApiController
    {
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<Authentication2faController> _logger;

        public Authentication2faController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<Authentication2faController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Authentication2faInformation>> Get2faInformation()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return new Authentication2faInformation
                       {
                           HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                           IsEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                           IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                           RecoveryCodesCount = await _userManager.CountRecoveryCodesAsync(user)
                       };
        }

        [HttpPost]
        [Route("ForgetClient")]
        public async Task<IActionResult> ForgetClient()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();
            return new OkObjectResult("The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.");
        }

        [HttpPost]
        [Route("GenerateRecoveryCodes")]
        public async Task<ActionResult<string[]>> GenerateRecoveryCodes()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            bool isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            string userId = await _userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.");
            }

            IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            _logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);

            return recoveryCodes.ToArray();
        }

        [HttpPost]
        [Route("Disable")]
        public async Task<IActionResult> Disable()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred disabling 2FA for user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", _userManager.GetUserId(User));
            return new OkObjectResult("2fa has been disabled. You can reenable 2fa when you setup an authenticator app");
        }

        [HttpPost]
        [Route("Reset")]
        public async Task<IActionResult> Reset()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

            await _signInManager.RefreshSignInAsync(user);

            return new OkObjectResult("Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.");
        }
    }
}
