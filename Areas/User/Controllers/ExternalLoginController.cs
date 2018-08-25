namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Services;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Models;

    [Area("User")]
    public class ExternalLoginController : AnonymousApiController
    {       
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public ExternalLoginController(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginController> logger, IIdentityServerInteractionService interaction, IEventService events)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _interaction = interaction;
            _events = events;
        }

        [HttpGet]
        [Route("providers")]
        public async Task<List<AuthenticationScheme>> GetAvailableExternalProviders()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Challenge(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            // start challenge and roundtrip the return URL and scheme 
            string redirectUri = Url.Action("Callback", new { returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [Route("callback")]
        public async Task<IActionResult> Callback(string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("Error loading external login information.");

                return new BadRequestObjectResult("Error loading external login information.");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                User user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.FirstName + " " + user.LastName));
                if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("~/");
            }

            if (result.RequiresTwoFactor)
            {
                // return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                // OR
                // return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }

            if (result.IsLockedOut)
            {
                return new BadRequestObjectResult("locked out");
            }

            // If the user does not have an account, then ask the user to create an account.
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                User user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));

                // Checks if user with this email already exists
                if (user != null)
                {
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (addLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.FirstName + " " + user.LastName));
                        if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }

                    return Redirect("external-login-error");
                }


                User userWithSameName = await _userManager.FindByNameAsync(info.Principal.GetDisplayName());
                User newUser;

                if (userWithSameName == null)
                {
                    newUser = new User { UserName = info.Principal.GetDisplayName(), Email = info.Principal.FindFirstValue(ClaimTypes.Email), EmailConfirmed = true };
                }
                else
                {
                    string differentName = info.Principal.FindFirstValue(ClaimTypes.Email).Substring(0, info.Principal.FindFirstValue(ClaimTypes.Email).IndexOf("@", StringComparison.Ordinal));
                    userWithSameName = await _userManager.FindByNameAsync(differentName);

                    while (userWithSameName != null)
                    {
                        const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        char[] stringChars = new char[8];
                        var random = new Random();

                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = Chars[random.Next(Chars.Length)];
                        }

                        differentName = new string(stringChars);
                        userWithSameName = await _userManager.FindByNameAsync(differentName.Normalize());
                    }

                    newUser = new User { UserName = differentName, Email = info.Principal.FindFirstValue(ClaimTypes.Email), EmailConfirmed = true };
                }

                var accountCreationResult = await _userManager.CreateAsync(newUser);

                if (accountCreationResult.Succeeded)
                {
                    var addLoginResult = await _userManager.AddLoginAsync(newUser, info);
                    if (addLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(newUser.UserName, newUser.Id.ToString(), newUser.FirstName + " " + newUser.LastName));
                        if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                }
            }
            else
            {
                // Change URL
                return Redirect("external-login-error");
            }

            return Redirect("external-login-error");
        }
    }
}
