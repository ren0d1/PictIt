namespace PictIt.Areas.User.Controllers
{
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Models;

    [Area("User")]
    public class ForgotPasswordController : AnonymousApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordController> _logger;

        public ForgotPasswordController(UserManager<User> userManager, IEmailSender emailSender, ILogger<ForgotPasswordController> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateRecoveryCode([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return StatusCode(209, "/home");
            }

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callbackUrl = $"https://{Request.Host.ToUriComponent()}/reset-password?code={HttpUtility.UrlEncode(code)}&email={HttpUtility.UrlEncode(email)}";

            await _emailSender.SendEmailAsync(
                email,
                "Reset Password",
                $"You can reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            return StatusCode(209, "/home");
        }
    }
}
