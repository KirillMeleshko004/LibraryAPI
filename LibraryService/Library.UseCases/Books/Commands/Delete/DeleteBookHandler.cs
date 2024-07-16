using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;

namespace Library.UseCases.Books.Commands
{
   public class DeleteBookHandler : IRequestHandler<DeleteBookCommand>
   {
      private readonly IRepositoryManager _repo;
      private readonly IImageService _images;
      private readonly ILogger<DeleteBookHandler> _logger;

      public DeleteBookHandler(IRepositoryManager repo, IImageService images,
         ILogger<DeleteBookHandler> logger)
      {
         _repo = repo;
         _images = images;
         _logger = logger;
      }
      public async Task Handle(DeleteBookCommand request,
         CancellationToken cancellationToken)
      {
         var book = await _repo.Books.GetBookByIdAsync(request.Id, cancellationToken);

         //If book not exist or already deleted
         if (book == null) return;

         await _repo.Books.DeleteBookAsync(book, cancellationToken);
         if (!string.IsNullOrWhiteSpace(book.ImagePath))
         {
            await _images.DeleteImageAsync(book.ImagePath!);
         }
         await _repo.SaveChangesAsync();


         _logger.LogInformation(BookDeletedLog, book.Id);
      }
   }
}