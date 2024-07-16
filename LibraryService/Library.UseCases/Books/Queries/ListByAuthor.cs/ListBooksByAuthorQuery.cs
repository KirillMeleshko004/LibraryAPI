using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.RequestFeatures;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksByAuthorQuery(BookParameters Parameters, Guid AuthorId) :
      IRequest<IEnumerable<BookDto>>;
}