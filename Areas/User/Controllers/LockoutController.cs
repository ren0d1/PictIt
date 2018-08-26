namespace PictIt.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Models;

    [Area("User")]
    public class LockoutController : AnonymousApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LockoutController> _logger;

        public LockoutController(UserManager<User> userManager, ILogger<LockoutController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRemainingLockoutTime(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"There is no existing user using {email} as an email address.");

                return new BadRequestObjectResult("wrong email/password");
            }

            return new OkObjectResult(await _userManager.GetLockoutEndDateAsync(user));
        }
    }
}
