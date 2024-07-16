using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;

namespace Library.UseCases.Authors.Commands
{
   public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand>
   {
      private readonly IRepositoryManager _repo;
      private readonly ILogger<DeleteAuthorHandler> _logger;

      public DeleteAuthorHandler(IRepositoryManager repo, ILogger<DeleteAuthorHandler> logger)
      {
         _repo = repo;
         _logger = logger;
      }

      public async Task Handle(DeleteAuthorCommand request,
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors.GetAuthorByIdAsync(request.Id, cancellationToken);

         if (author == null) return;

         await _repo.Authors.DeleteAuthorAsync(author, cancellationToken);
         await _repo.SaveChangesAsync();

         _logger.LogInformation(AuthorDeletedLog, author.Id);
      }
   }
}