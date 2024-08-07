using Identity.UseCases.Users.Dtos;
using MediatR;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Command for user creation
   /// </summary>
   /// <returns>Identity Result</returns>
   public record CreateUserCommand(UserForCreationDto UserDto) :
      IRequest
   { }
}