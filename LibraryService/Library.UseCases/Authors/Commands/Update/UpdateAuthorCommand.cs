using Library.UseCases.Authors.DTOs;
using MediatR;

namespace Library.UseCases.Authors.Commands
{
   public record UpdateAuthorCommand(Guid AuthorId, AuthorForUpdateDto AuthorDto) :
      IRequest<AuthorDto>;
}