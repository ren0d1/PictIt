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
        public async Task<IActionResult> AuthenticatorCodeCheck([FromForm] Login2faUser authenticationUser)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("~/");
            }

            authenticationUser.ReturnUrl = authenticationUser.ReturnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = authenticationUser.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, false, authenticationUser.RememberMachineBool);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(authenticationUser.ReturnUrl);
            }
            
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            
             _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Redirect("~/");
        }  
    }
}
