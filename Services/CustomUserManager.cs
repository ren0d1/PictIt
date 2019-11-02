namespace PictIt.Services
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using PictIt.Models;

    public class CustomUserManager : UserManager<User>
    {
        public CustomUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, new IUserValidator<User>[] { new CustomUserValidator() }, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
