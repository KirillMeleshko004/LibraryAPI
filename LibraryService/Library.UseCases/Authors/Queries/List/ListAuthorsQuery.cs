using Library.Shared.Results;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.RequestFeatures;
using MediatR;

namespace Library.UseCases.Authors.Queries
{
   public record ListAuthorsQuery(AuthorParameters Parameters) : 
      IRequest<Result<IEnumerable<AuthorDto>>> {}
}