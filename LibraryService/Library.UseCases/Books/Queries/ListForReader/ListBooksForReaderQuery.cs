using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using MediatR;

namespace Library.UseCases.Books.Queries
{
   public record ListBooksForReaderQuery(RequestParameters Parameters, string ReaderEmail) :
      IRequest<IPagedEnumerable<BookDto>>;

}