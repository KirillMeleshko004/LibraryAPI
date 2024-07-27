using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

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
         var author = await _repo.Authors.GetSingle(a => a.Id.Equals(request.Id),
            cancellationToken: cancellationToken);

         if (author == null) return;

         _repo.Authors.Delete(author, cancellationToken);
         await _repo.SaveChangesAsync();
      }
   }
}