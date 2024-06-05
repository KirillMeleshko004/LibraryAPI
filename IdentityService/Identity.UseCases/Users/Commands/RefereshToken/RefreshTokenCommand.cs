using Identity.Shared.Results;
using Identity.UseCases.Common.Tokens;
using MediatR;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Command for token refreshing
   /// </summary>
   /// <returns>Result with new token. Possible Result.Status: Ok, Unauthorized</returns>
   public record RefreshTokenCommand(Token ExpiredToken) : IRequest<Result<Token>> { }
}