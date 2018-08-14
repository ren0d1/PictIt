namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Events;
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
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{info.Principal.Identity.Name} logged in with {info.LoginProvider} provider.");
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return new BadRequestObjectResult("locked out");
            }
            else
            {
                var user = await _userManager.FindByEmailAsync("mytestemail@email.com");
                var test = await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }
    }
}
