using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Books.Commands
{
   public record BorrowBookCommand(string ReaderEmail, Guid BookId) :
      IRequest<Result>;
}