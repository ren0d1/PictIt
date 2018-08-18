namespace PictIt.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Models;

    [Area("User")]
    public class LogoutController : AnonymousApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(SignInManager<User> signInManager, ILogger<LogoutController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }
    }
}
