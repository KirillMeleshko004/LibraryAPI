using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksQuery : IRequest<IEnumerable<BookDto>> {}
}