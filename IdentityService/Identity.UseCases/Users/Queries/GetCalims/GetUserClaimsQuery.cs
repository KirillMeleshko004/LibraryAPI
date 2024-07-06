using System.Security.Claims;
using Identity.Shared.Results;
using MediatR;

namespace Identity.UseCases.Users.Queries
{
    public record GetUserClaimsQuery(string UserName,
        IEnumerable<string> Scopes) : IRequest<Result<ClaimsIdentity>>
    { }
}