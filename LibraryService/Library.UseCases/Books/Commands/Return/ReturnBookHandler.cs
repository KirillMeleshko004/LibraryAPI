using Library.Shared.Results;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;
using static Library.UseCases.Common.Messages.LoggingMessages;

namespace Library.UseCases.Books.Commands
{
   public class ReturnBookHandler : IRequestHandler<ReturnBookCommand, Result>
   {
      private readonly IRepositoryManager _repo;
      private readonly ILogger<ReturnBookHandler> _logger;

      public ReturnBookHandler(IRepositoryManager repo, ILogger<ReturnBookHandler> logger)
      {
         _repo = repo;
         _logger = logger;
      }

      public async Task<Result> Handle(ReturnBookCommand request,
         CancellationToken cancellationToken)
      {
         var reader = await _repo.Readers
            .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);

         if (reader == null)
         {
            _logger.LogInformation(ReaderNotFoundLog, request.ReaderEmail);
            return Result.NotFound(ReaderNotFound);
         }

         var book = await _repo.Books.GetBookByIdAsync(request.BookId, cancellationToken);

         if (book == null)
         {
            return Result.NotFound(
               string.Format(ResponseMessages.BookNotFound, request.BookId));
         }

         if (book.CurrentReaderId != reader.Id)
         {
            _logger.LogInformation(ReaderDontHaveBookLog,
               request.ReaderEmail, request.BookId);
            return Result.InvalidData(ReaderDontHaveBook);
         }

         book.IsAvailable = true;
         book.CurrentReaderId = null;
         book.BorrowTime = null;
         book.ReturnTime = null;

         await _repo.SaveChangesAsync();

         return Result.Success();
      }
   }
}