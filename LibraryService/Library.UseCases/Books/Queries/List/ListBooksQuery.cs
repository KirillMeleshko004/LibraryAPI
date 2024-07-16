using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.RequestFeatures;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksQuery(BookParameters Parameters) :
      IRequest<IEnumerable<BookDto>>;
}