using Library.UseCases.Authors.DTOs;
using MediatR;

namespace Library.UseCases.Authors.Commands
{
   public record CreateAuthorCommand(AuthorForCreationDto AuthorDto) :
      IRequest<AuthorDto>;
}