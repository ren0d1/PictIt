namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;
    using PictIt.Models;

    [Area("User")]
    public class Login2faController : AnonymousApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<Login2faController> _logger;

        public Login2faController(SignInManager<User> signInManager, ILogger<Login2faController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticatorCodeCheck([FromBody] Login2faUser authenticationUser)
        {
            authenticationUser.ReturnUrl = authenticationUser.ReturnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException("Unable to load two-factor authentication user.");
            }

            string authenticatorCode = authenticationUser.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, false, authenticationUser.RememberMachineBool);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return StatusCode(209, authenticationUser.ReturnUrl);
            }
            
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return StatusCode(209, $"/lockout?email={user.Email}");
            }
            
             _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
            return UnprocessableEntity("Code is invalid.");
        }

        [HttpPost("RecoveryCode")]
        public async Task<IActionResult> OnPostAsync([FromBody] Recovery2fa recovery2faLogin)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException("Unable to load two-factor authentication user.");
            }

            string recoveryCode = recovery2faLogin.RecoveryCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
                
            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return StatusCode(209, recovery2faLogin.ReturnUrl ?? Url.Content("/home"));
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return StatusCode(209, $"/lockout?email={user.Email}");
            }
            
            _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
            return UnprocessableEntity("Recovery code is invalid.");
        }
    }
}
