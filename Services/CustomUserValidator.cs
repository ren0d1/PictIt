namespace PictIt.Services
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using PictIt.Models;

    public class CustomUserValidator : UserValidator<User>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager,  User user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            Regex userNameRegex = new Regex(@"^[\p{L}\p{S}\p{N}\p{P}\p{Cs}]+( [\p{L}\p{S}\p{N}\p{P}\p{Cs}]+)*$", RegexOptions.Compiled);

            if (!userNameRegex.IsMatch(user.UserName))
            {
                IdentityError invalidUserNameError = Describer.InvalidUserName(user.UserName);
                invalidUserNameError.Description += " Username doesn't match the expected format.";
                errors.Add(invalidUserNameError);
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
