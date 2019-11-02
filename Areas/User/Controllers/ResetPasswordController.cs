namespace PictIt.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;
    using PictIt.Models;

    [Area("User")]
    public class ResetPasswordController : AnonymousApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ForgotPasswordController> _logger;

        public ResetPasswordController(UserManager<User> userManager, ILogger<ForgotPasswordController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SetNewPassword([FromBody] ResetPasswordUser resetPasswordUser)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordUser.Email);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordUser.Code, resetPasswordUser.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult("Password successfully reset.");
            }

            return BadRequest();
        }
    }
}
