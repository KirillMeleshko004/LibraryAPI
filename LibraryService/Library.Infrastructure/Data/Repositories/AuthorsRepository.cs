using Library.Domain.Entities;
using Library.Infrastructure.Data.Extensions;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
   /// <summary>
   /// Repository to manipulate authors in data storage
   /// </summary>
   public class AuthorsRepository : RepositoryBase<Author>, IAuthorRepository
   {
      public AuthorsRepository(RepositoryContext context) : base(context) {}

      public async Task<Author?> GetAuthorByIdAsync(Guid id, 
         CancellationToken cancellationToken)
      {
         return await Get(a => a.Id.Equals(id))
            .SingleOrDefaultAsync(cancellationToken);
      }

      public async Task<IEnumerable<Author>> GetAuthorsAsync(AuthorParameters parameters, 
         CancellationToken cancellationToken)
      {
         return await Get()
            .FilterAuthors(parameters)
            .Sort(parameters.OrderBy)
            .Page(parameters)
            .ToListAsync(cancellationToken);
      }

      public async Task AddAuthorAsync(Author author, 
         CancellationToken cancellationToken)
      {
         Create(author);

         await Task.CompletedTask;
      }

      public async Task DeleteAuthorAsync(Author author, 
         CancellationToken cancellationToken)
      {
         Delete(author);

         await Task.CompletedTask;
      }

      public async Task UpdateAuthorAsync(Author author, 
         CancellationToken cancellationToken)
      {
         Update(author);

         await Task.CompletedTask;
      }

   }
}