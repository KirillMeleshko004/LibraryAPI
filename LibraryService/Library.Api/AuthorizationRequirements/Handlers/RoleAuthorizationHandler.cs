using Library.Api.AuthorizationRequirements.Requirements;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;

namespace Library.Api.AuthorizationRequirements.Handlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleAuthorizationRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == OpenIddictConstants.Claims.Role))
            {
                context.Fail(new AuthorizationFailureReason(this, "Missing role claim."));
                return Task.CompletedTask;
            }

            var roleClaims = context.User.Claims
                .Where(c => c.Type == OpenIddictConstants.Claims.Role);

            var roles = roleClaims.Select(c => c.Value);
            var expectedRole = requirement.RequiredRole;

            if (!roles.Any(r => r.Equals(expectedRole, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Fail(new AuthorizationFailureReason(this, "Missing required role for action."));
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}