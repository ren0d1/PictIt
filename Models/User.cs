namespace PictIt.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser<Guid>, IModel<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
