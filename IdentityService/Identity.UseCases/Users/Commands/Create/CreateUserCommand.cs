using Identity.UseCases.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.UseCases.Users.Commands
{
   public record CreateUserCommand(UserForCreationDto UserDto) : 
      IRequest<(IdentityResult result, UserDto? user)>{}
}