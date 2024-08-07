using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

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
         var book = await _repo.Books
            .GetSingle(b => b.Id.Equals(request.Id), cancellationToken: cancellationToken);

         //If book not exist or already deleted
         if (book == null) return;

         _repo.Books.Delete(book, cancellationToken);
         if (!string.IsNullOrWhiteSpace(book.ImagePath))
         {
            await _images.DeleteImageAsync(book.ImagePath!);
         }
         await _repo.SaveChangesAsync();

         _logger.LogInformation("Book with id: {id} was deleted.", book.Id);
      }
   }
}