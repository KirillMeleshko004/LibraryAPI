using Library.UseCases.Common.Interfaces;
using Library.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;

namespace Library.UseCases.Books.Commands
{
   public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Result>
   {
      private readonly IRepositoryManager _repo;
      private readonly ILogger _logger;

      public DeleteBookHandler(IRepositoryManager repo, ILogger logger)
      {
         _repo = repo;
         _logger = logger;
      }
      public async Task<Result> Handle(DeleteBookCommand request, 
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetBookByIdAsync(request.Id, cancellationToken);

         //If book not exist or already deleted
         if (book == null) return Result.Success();

         await _repo.Books.DeleteBookAsync(book, cancellationToken);
         await _repo.SaveChangesAsync();

         _logger.LogInformation(BookDeletedLog, book.Id);

         return Result.Success();
      }
   }
}