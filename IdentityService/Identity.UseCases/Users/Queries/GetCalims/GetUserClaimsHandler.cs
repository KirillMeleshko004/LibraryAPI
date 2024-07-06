using System.Security.Claims;
using Identity.Domain.Entities;
using Identity.UseCases.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.UseCases.Users.Queries
{
    public class GetUserClaimsHandler :
        IRequestHandler<GetUserClaimsQuery, ClaimsIdentity>
    {
        private readonly UserManager<User> _userManager;

        public GetUserClaimsHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsIdentity> Handle(GetUserClaimsQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid username/password pair");
            }

            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Name
            );

            var roles = await _userManager.GetRolesAsync(user);

            identity
                .SetClaim(Claims.Subject,
                    await _userManager.GetUserIdAsync(user))
                .SetClaim(Claims.Name,
                    await _userManager.GetUserNameAsync(user))
                .SetClaim(Claims.Email, await _userManager.GetEmailAsync(user))
                .SetClaim(Claims.GivenName, user.FirstName)
                .SetClaim(Claims.FamilyName, user.LastName)
                .SetClaim(Claims.Audience, "gameshop")
                .SetClaims(Claims.Role, [.. (await _userManager.GetRolesAsync(user))]);

            identity.SetScopes(request.Scopes);
            identity.SetDestinations(DestinationsSelector);

            return identity;
        }

        private static IEnumerable<string> DestinationsSelector(Claim claim)
        {
            switch (claim.Type)
            {
                case Claims.Name or Claims.PreferredUsername:
                case Claims.Role:
                case Claims.Email:
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;

                    yield break;

                case Claims.GivenName:
                case Claims.FamilyName:
                    yield return Destinations.IdentityToken;
                    yield break;

                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}