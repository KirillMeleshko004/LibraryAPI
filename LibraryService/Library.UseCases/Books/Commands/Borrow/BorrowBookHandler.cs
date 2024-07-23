using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Books.Commands
{
   public class BorrowBookHandler : IRequestHandler<BorrowBookCommand>
   {
      private readonly IRepositoryManager _repo;
      private readonly ILogger<BorrowBookHandler> _logger;

      public BorrowBookHandler(IRepositoryManager repo, ILogger<BorrowBookHandler> logger)
      {
         _repo = repo;
         _logger = logger;
      }

      public async Task Handle(BorrowBookCommand request, CancellationToken cancellationToken)
      {
         var book = await _repo.Books
            .GetBookByIdAsync(request.BookId, cancellationToken);

         if (book == null)
         {
            _logger.LogWarning(BookNotFoundLog, request.BookId);
            throw new NotFoundException(string.Format(BookNotFound, request.BookId));
         }

         if (book.IsAvailable == false)
         {
            throw new UnavailableException(string.Format(BookNotAvailable, request.BookId));
         }

         var reader = await _repo.Readers
            .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);

         if (reader == null)
         {
            await _repo.Readers.AddReaderAsync(request.ReaderEmail, cancellationToken);
            await _repo.SaveChangesAsync();
            reader = await _repo.Readers
               .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);
         }

         book.IsAvailable = false;
         book.CurrentReaderId = reader!.Id;
         book.BorrowTime = DateTime.Now;
         book.ReturnTime = DateTime.Now.AddDays(30);

         //Test if necessary
         await _repo.Books.UpdateBookAsync(book, cancellationToken);
         await _repo.SaveChangesAsync();
      }
   }
}