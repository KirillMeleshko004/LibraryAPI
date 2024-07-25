using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using MediatR;

namespace Library.UseCases.Authors.Queries
{
   /// <summary>
   /// Query for authors retrieval. Possible response statuses: Ok, NotFound.
   /// </summary>
   public record ListAuthorsQuery(RequestParameters Parameters) :
      IRequest<IPagedEnumerable<AuthorDto>>
   { }
}