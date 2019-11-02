namespace PictIt.Areas.User.Models
{
    public class RegistrationUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool AcceptedTerms { get; set; }
    }
}
