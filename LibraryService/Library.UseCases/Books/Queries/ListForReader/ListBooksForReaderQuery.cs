using Library.Shared.Results;
using Library.UseCases.Books.DTOs;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksForReaderQuery(string ReaderEmail) :
      IRequest<Result<IEnumerable<BookDto>>>
   { }

}