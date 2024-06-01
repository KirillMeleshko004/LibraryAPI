using Identity.Shared.Results;
using Identity.UseCases.Common.Tokens;
using MediatR;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Command for token refreshing
   /// </summary>
   /// <param name="ExpiredToken">Expired token send from user</param>
   /// <returns>Result with new token. Possible Result.Status: Ok, Unauthorized</returns>
   public record RefreshTokenCommand(Token ExpiredToken) : IRequest<Result<Token>> {}
}