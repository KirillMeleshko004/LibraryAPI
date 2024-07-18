using Microsoft.AspNetCore.Authorization;

namespace Library.Api.AuthorizationRequirements.Requirements
{
    public class RoleAuthorizationRequirement(string requiredRole) :
        IAuthorizationRequirement
    {
        public string RequiredRole { get; } = requiredRole;
    }
}