using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using MediatR;

namespace Library.UseCases.Authors.Queries
{
   /// <summary>
   /// Query for author retrieval by id. Possible response statuses: Ok, NotFound.
   /// </summary>
   public record GetAuthorByIdQuery(Guid Id) : IRequest<Result<AuthorDto>> { }
}