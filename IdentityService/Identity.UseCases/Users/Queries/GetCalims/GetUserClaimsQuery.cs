using System.Security.Claims;
using MediatR;

namespace Identity.UseCases.Users.Queries
{
    public record GetUserClaimsQuery(string UserName,
        IEnumerable<string> Scopes) : IRequest<ClaimsIdentity>
    { }
}