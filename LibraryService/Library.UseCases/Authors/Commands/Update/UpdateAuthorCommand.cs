using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using MediatR;

namespace Library.UseCases.Authors.Commands
{
   /// <summary>
   /// Command for author update. Possible response statuses: Ok, NotFound.
   /// </summary>
   public record UpdateAuthorCommand(Guid AuthorId, AuthorForUpdateDto AuthorDto) :
      IRequest<Result<AuthorDto>>
   { }
}