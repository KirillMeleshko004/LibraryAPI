using Library.Domain.Entities;
using Library.UseCases.Common.RequestFeatures;

namespace Library.UseCases.Common.Interfaces
{
   public interface IAuthorRepository
   {
      Task<IEnumerable<Author>> GetAuthorsAsync(AuthorParameters parameters, 
         CancellationToken cancellationToken);
      Task<Author?> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken);

      Task AddAuthorAsync(Author author, CancellationToken cancellationToken);
      Task UpdateAuthorAsync(Author author, CancellationToken cancellationToken);
      Task DeleteAuthorAsync(Author author, CancellationToken cancellationToken);

   }
}