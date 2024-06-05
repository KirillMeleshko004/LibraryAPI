using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using MediatR;

namespace Library.UseCases.Authors.Commands
{
   /// <summary>
   /// Command for author creation. Possible response statuses: Ok.
   /// </summary>
   public record CreateAuthorCommand(AuthorForCreationDto AuthorDto) :
      IRequest<Result<AuthorDto>>
   { }
}