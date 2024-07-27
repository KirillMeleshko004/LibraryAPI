using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;
using Library.UseCases.Exceptions;

namespace Library.UseCases.Books.Commands
{
   public class ReturnBookHandler : IRequestHandler<ReturnBookCommand>
   {
      private readonly IRepositoryManager _repo;
      private readonly ILogger<ReturnBookHandler> _logger;

      public ReturnBookHandler(IRepositoryManager repo, ILogger<ReturnBookHandler> logger)
      {
         _repo = repo;
         _logger = logger;
      }

      public async Task Handle(ReturnBookCommand request,
         CancellationToken cancellationToken)
      {
         var reader = await _repo.Readers
            .GetSingle(r => r.Email.Equals(request.ReaderEmail),
               cancellationToken: cancellationToken);

         if (reader == null)
         {
            throw new UnauthorizedException(string.Format(ReaderNotFound, request.ReaderEmail));
         }

         var book = await _repo.Books
            .GetSingle(b => b.Id.Equals(request.BookId), cancellationToken: cancellationToken);

         if (book == null)
         {
            throw new NotFoundException(
               string.Format(BookNotFound, request.BookId));
         }

         if (book.CurrentReaderId != reader.Id)
         {
            throw new ForbidException(
               string.Format(ReaderDontHaveBook, request.ReaderEmail, request.BookId));
         }

         book.IsAvailable = true;
         book.CurrentReaderId = null;
         book.BorrowTime = null;
         book.ReturnTime = null;

         _repo.Books.Update(book, cancellationToken);
         await _repo.SaveChangesAsync();
      }
   }
}