namespace PictIt.Areas.User.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class ConsentUser
    {
        [JsonIgnore]
        public List<string> GrantedScopes { get; set; } = new List<string>
                                                              {
                                                                  "pict-it-api.read"
                                                              };

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        // All requested identity scopes
        public string OpenId
        {
            get => string.Empty;

            set
            {
                if (value.Equals("on") || value.Equals("yes") || value.Equals("true"))
                    GrantedScopes.Add("openid");
            }
        }

        public string Profile
        {
            get => string.Empty;

            set
            {
                if (value.Equals("on") || value.Equals("yes") || value.Equals("true"))
                    GrantedScopes.Add("profile");
            }
        }

        public string Email
        {
            get => string.Empty;

            set
            {
                if (value.Equals("on") || value.Equals("yes") || value.Equals("true"))
                    GrantedScopes.Add("email");
            }
        }
    }
}
