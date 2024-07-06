using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Controllers.Helpers
{
    internal class PasswordFlowHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PasswordFlowHelper(UserManager<User> userManager,
           SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        internal static IActionResult InvalidCredentialsResponse()
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                    Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                       "The username/password couple is invalid."
            });

            return new ForbidResult
            (
                properties: properties,
                authenticationScheme: OpenIddictServerAspNetCoreDefaults
                    .AuthenticationScheme
            );
        }
    }
}