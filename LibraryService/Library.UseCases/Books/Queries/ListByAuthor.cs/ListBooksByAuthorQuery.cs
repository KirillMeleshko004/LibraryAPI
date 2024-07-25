using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksByAuthorQuery(RequestParameters Parameters, Guid AuthorId) :
      IRequest<IPagedEnumerable<BookDto>>;
}