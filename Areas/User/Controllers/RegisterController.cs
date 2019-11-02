namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;
    using PictIt.Models;

    [Area("User")]
    public class RegisterController : AnonymousApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(UserManager<User> userManager, IEmailSender emailSender, ILogger<RegisterController> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        [Route("Email")]
        public async Task<string> IsEmailReachable(string email)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"http://api.emailcrawlr.com/v2/email/verify?email={email}");
                message.Headers.Add("Accept", "application/json");
                message.Headers.Add("x-api-key", "e7659ebf65e893fdf77702e58d28e701 ");

                HttpResponseMessage response = await client.SendAsync(message);
                return await response.Content.ReadAsStringAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationUser userToRegister)
        {
            Regex emailRegex = new Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", RegexOptions.Compiled);
            if (emailRegex.IsMatch(userToRegister.Email))
            {
                Regex passwordRegex = new Regex(@"(?=.*\p{N})(?=.*\p{Ll})(?=.*\p{Lu})(?=.*[\p{P}\p{S}\p{Cs}]).{8,}");
                if (passwordRegex.IsMatch(userToRegister.Password))
                {
                    if (userToRegister.AcceptedTerms)
                    {
                        if (userToRegister.FirstName.Length > 0 && userToRegister.LastName.Length > 0)
                        {
                            Regex nameRegex = new Regex(@"^(([\p{Lt}\p{Lu}][\p{Ll}]+)|(\p{Lm})|(\p{Lo}))+([ ]?(([\p{Lt}\p{Lu}][\p{Ll}]+)|(\p{Lm})|(\p{Lo})))*$", RegexOptions.Compiled);
                            if (!nameRegex.IsMatch(userToRegister.FirstName) && !nameRegex.IsMatch(userToRegister.LastName))
                            {
                                _logger.LogInformation("Either first or last name doesn't match the expected format.");
                                return new BadRequestObjectResult(userToRegister);
                            }
                        }
                        else
                        {
                            if (userToRegister.UserName.Length > 0)
                            {
                                Regex userNameRegex = new Regex(@"^[\p{L}\p{S}\p{N}\p{P}\p{Cs}]+( [\p{L}\p{S}\p{N}\p{P}\p{Cs}]+)*$", RegexOptions.Compiled);
                                if (!userNameRegex.IsMatch(userToRegister.UserName))
                                {
                                    _logger.LogInformation($"Username '{userToRegister.UserName}' doesn't match the expected format.");
                                    return new BadRequestObjectResult(userToRegister);
                                }
                            }
                            else
                            {
                                _logger.LogInformation("Missing a first name, a last name or a username.");
                                return new BadRequestObjectResult(userToRegister);
                            }
                        }

                        User newUser = new User
                                           {
                                               FirstName = userToRegister.FirstName,
                                               LastName = userToRegister.LastName,
                                               UserName = userToRegister.UserName.Length > 0 ? userToRegister.UserName : userToRegister.LastName + userToRegister.FirstName,
                                               Email = userToRegister.Email
                                           };

                        IdentityResult result = await _userManager.CreateAsync(newUser, userToRegister.Password);
                        if (result.Succeeded)
                        {
                            string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                            string callbackUrl = $"https://{Request.Host.ToUriComponent()}/api/user/ConfirmEmail?userId={newUser.Id}&code={HttpUtility.UrlEncode(code)}";

                            await _emailSender.SendEmailAsync(
                                newUser.Email,
                                "Confirm your email to finish your registration",
                                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                            _logger.LogInformation($"'{userToRegister.Email}' successfully registered.");
                            return new JsonResult(newUser.Id);
                        }

                        _logger.LogError($"The following internal error(s) prevented registration : {result.Errors}.");
                        return new BadRequestObjectResult(result.Errors);
                    }

                    _logger.LogInformation("Terms and conditions haven't been accepted.");
                    return new BadRequestObjectResult(userToRegister);
                }

                _logger.LogInformation("Entered password doesn't match the expected format.");
                return new BadRequestObjectResult(userToRegister);
            }

            _logger.LogInformation($"Email '{userToRegister.Email}' doesn't match the expected format.");
            return new BadRequestObjectResult(userToRegister);
        }
    }
}
