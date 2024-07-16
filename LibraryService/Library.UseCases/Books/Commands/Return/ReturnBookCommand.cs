using MediatR;

namespace Library.UseCases.Books.Commands
{
   public record ReturnBookCommand(string ReaderEmail, Guid BookId)
      : IRequest;
}