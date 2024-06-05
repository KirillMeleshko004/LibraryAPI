using Library.Shared.Results;
using MediatR;

namespace Library.UseCases.Books.Commands
{
   /// <summary>
   /// Command to borrow book. Possible response statuses: Ok, NotFound.
   /// </summary>
   public record BorrowBookCommand(string ReaderEmail, Guid BookId) :
      IRequest<Result>;
}