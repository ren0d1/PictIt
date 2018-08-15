namespace PictIt.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Models;

    [Area("User")]
    public class ConfirmEmailController : AnonymousApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ConfirmEmailController> _logger;

        public ConfirmEmailController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<ConfirmEmailController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                _logger.LogError("Missing user id or confirmation code.");
                return new BadRequestObjectResult("Missing user id or confirmation code.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"Couldn't find user with the following given id : {userId}.");
                return new BadRequestObjectResult("Couldn't find user with the given id.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                _logger.LogError($"Internal error(s) prevented email confirmation: {result.Errors}.");
                return new BadRequestObjectResult(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogTrace($"{userId} successfully confirmed his email.");
            return new OkObjectResult("Successfully confirmed email address.");
        }
    }
}
