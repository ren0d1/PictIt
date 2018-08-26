namespace PictIt.Areas.User.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;
    using PictIt.Models;

    [Area("User")]
    public class AuthenticatorController : AuthorizedApiController
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticatorController> _logger;
        private readonly UrlEncoder _urlEncoder;

        public AuthenticatorController(UserManager<User> userManager, ILogger<AuthenticatorController> logger, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [HttpGet]
        public async Task<ActionResult<AuthenticatorUser>> GetSharedKeyAndQrCodeUri()
        {
            User user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return await LoadSharedKeyAndQrCodeUriAsync(user);
        }

        [HttpPost]
        public async Task<IActionResult> EnableAuthenticator([FromQuery] string twoFactorCode)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Strip spaces and hypens
            string verificationCode = twoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            bool is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                return UnprocessableEntity("Verification code is invalid.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            string userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                string[] recoveryCodesArray = recoveryCodes.ToArray();
                string redirectUrl = $"/show-codes?{recoveryCodesArray.Aggregate(string.Empty, (result, item) => result + (result.Length > 0 ? "&" : string.Empty) + "recoveryCodes=" + item)}";
                return StatusCode(209, redirectUrl);
            }

            return StatusCode(209, "/profile");
        }

        private async Task<AuthenticatorUser> LoadSharedKeyAndQrCodeUriAsync(User user)
        {
            // Load the authenticator key & QR code URI
            string unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            string email = await _userManager.GetEmailAsync(user);
            return new AuthenticatorUser
                       {
                           SharedKey = FormatKey(unformattedKey),
                           AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey)
                       };
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("PictIt"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
