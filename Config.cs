namespace PictIt
{
    using System.Collections.Generic;

    using IdentityServer4.Models;

    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
                       {
                           new IdentityResources.OpenId(),
                           new IdentityResources.Profile(),
                           new IdentityResources.Email(),
                           new IdentityResources.Address(),
                           new IdentityResources.Phone()
                       };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
                       {
                           new ApiResource("pict-it-api", "PictIt API")
                               {
                                   Scopes =
                                       {
                                           new Scope("pict-it-api.read"),
                                           new Scope("pict-it-api.write")
                                       }
                               }
                       };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
                       {
                           new Client
                               {
                                   ClientId = "pict_it_spa",
                                   ClientName = "PictIt",
                                   AllowedGrantTypes = GrantTypes.Implicit,
                                   AllowedScopes = 
                                       {
                                           "openid", "profile", "email", "pict-it-api.read" 
                                       },
                                   RedirectUris = 
                                       {
                                          "https://localhost:44399/auth-callback" 
                                       },
                                   PostLogoutRedirectUris = 
                                       {
                                            "https://localhost:44399/" 
                                       },
                                   AllowedCorsOrigins = 
                                       {
                                            "https://localhost:44399/" 
                                       },
                                   AllowAccessTokensViaBrowser = true,
                                   AccessTokenLifetime = 3600
                               }
                       };
        }
    }
}
