namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using PictIt.Areas.User.Models;

    [Area("User")]
    public class ConsentController : AnonymousApiController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(IIdentityServerInteractionService interaction, IClientStore clientStore, IResourceStore resourceStore, ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsentRequest(string returnUrl)
        {
            var authorizationRequest = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (authorizationRequest != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(authorizationRequest.ClientId);
                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(authorizationRequest.ScopesRequested);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        ConsentRequest consentRequest = new ConsentRequest(authorizationRequest, client.ClientName);
                        IEnumerable<IdentityResource> requestedIdentityScopes = resources.IdentityResources.Where(x => consentRequest.ScopesRequested.Contains(x.Name));
                        return Ok(new { consentRequest, requestedIdentityScopes });
                    }

                    _logger.LogError($"No scopes matching: {authorizationRequest.ScopesRequested.Aggregate((x, y) => x + ", " + y)}");
                    return new BadRequestObjectResult("No scopes matching");
                }

                _logger.LogError($"Invalid client id: {authorizationRequest.ClientId}");
                return new BadRequestObjectResult("Invalid client id");
            }

            _logger.LogError($"No consent request matching request: {returnUrl}");
            return new BadRequestObjectResult("No consent request matching request");
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUserConsent([FromForm] ConsentUser userConsent)
        {
            // validate return url is still valid
            var request = await _interaction.GetAuthorizationContextAsync(userConsent.ReturnUrl);
            if (request == null) return Redirect("~/");

            /*
             * Add logic for consent not granted
             */

            // communicate outcome of consent back to identityserver
           ConsentResponse userConsentResponse = new ConsentResponse
                                                      {
                                                          RememberConsent = userConsent.RememberMe,
                                                          ScopesConsented = userConsent.GrantedScopes
                                                      };
            await _interaction.GrantConsentAsync(request, userConsentResponse);

            if (_interaction.IsValidReturnUrl(userConsent.ReturnUrl) || Url.IsLocalUrl(userConsent.ReturnUrl))
            {
                return Redirect(userConsent.ReturnUrl);
            }

            return Redirect("~/");
        }
    }
}
