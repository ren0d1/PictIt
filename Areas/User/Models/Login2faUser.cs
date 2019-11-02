namespace PictIt.Areas.User.Models
{
    using Newtonsoft.Json;

    public class Login2faUser
    {
        public string TwoFactorCode { get; set; }

        [JsonIgnore]
        public bool RememberMachineBool { get; set; }

        public string RememberMachine
        {
            get => string.Empty;

            set
            {
                if (value.Equals("on") || value.Equals("yes") || value.Equals("true"))
                    RememberMachineBool = true;
            }
        }

        public string ReturnUrl { get; set; }
    }
}
