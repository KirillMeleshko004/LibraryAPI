using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;
using static Library.UseCases.Common.Messages.LoggingMessages;
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
            .GetReaderByEmailAsync(request.ReaderEmail, cancellationToken);

         if (reader == null)
         {
            _logger.LogInformation(ReaderNotFoundLog, request.ReaderEmail);
            throw new UnauthorizedException("Invalid reader");
         }

         var book = await _repo.Books.GetBookByIdAsync(request.BookId, cancellationToken);

         if (book == null)
         {
            throw new NotFoundException(
               string.Format(ResponseMessages.BookNotFound, request.BookId));
         }

         if (book.CurrentReaderId != reader.Id)
         {
            _logger.LogInformation(ReaderDontHaveBookLog,
               request.ReaderEmail, request.BookId);

            throw new ForbidException(ReaderDontHaveBook);
         }

         book.IsAvailable = true;
         book.CurrentReaderId = null;
         book.BorrowTime = null;
         book.ReturnTime = null;

         await _repo.SaveChangesAsync();
      }
   }
}