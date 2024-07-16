using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksForReaderQuery(string ReaderEmail) :
      IRequest<IEnumerable<BookDto>>;

}