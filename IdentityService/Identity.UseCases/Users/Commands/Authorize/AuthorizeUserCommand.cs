using Identity.Shared.Results;
using Identity.UseCases.Tokens.Dtos;
using Identity.UseCases.Users.Dtos;
using MediatR;

namespace Identity.UseCases.Users.Commands
{
   public record AuthorizeUserCommand(UserForAuthorizationDto UserDto) : 
      IRequest<Result<TokenDto>> {}
}