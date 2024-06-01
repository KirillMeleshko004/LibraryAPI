using Identity.UseCases.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Command for user creation
   /// </summary>
   /// <param name="UserDto"></param>
   /// <returns>Identity Result</returns>
   public record CreateUserCommand(UserForCreationDto UserDto) : 
      IRequest<IdentityResult>{}
}