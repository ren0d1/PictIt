namespace PictIt.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser<Guid>, IModel<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid PersonId { get; set; }

        public List<Picture> Pictures { get; set; }

        public List<Search> Searches { get; set; }
    }
}
