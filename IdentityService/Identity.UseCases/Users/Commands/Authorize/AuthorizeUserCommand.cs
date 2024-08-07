using Identity.UseCases.Users.Dtos;
using MediatR;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Command for user authorization
   /// </summary>
   /// <returns>Result with token. Possible Result.Status: Ok, Unauthorized</returns>
   public record AuthorizeUserCommand(UserForAuthorizationDto UserDto) :
      IRequest
   { }
}