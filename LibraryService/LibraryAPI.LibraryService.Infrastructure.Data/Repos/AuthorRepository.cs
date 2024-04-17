using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;
using LibraryAPI.LibraryService.Infrastructure.Data.Extensions;
using LibraryAPI.LibraryService.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Repos
{
   /// <summary>
   /// Repository to manipulate authors in data storage
   /// </summary>
   public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
   {
      public AuthorRepository(RepositoryContext context) : base(context)
      {
      }

      public async Task<Author?> GetAuthorByIdAsync(Guid id, bool trackChanges)
      {
         return await Get(a => a.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
      }

      public async Task<IEnumerable<Author>> GetAuthorsAsync(AuthorParameters parameters,
         bool trackChanges)
      {
         return await Get(trackChanges)
            .FilterAuthors(parameters)
            .Sort(parameters.OrderBy)
            .Page(parameters)
            .ToListAsync();
      }

      public async Task AddAuthorAsync(Author author)
      {
         Create(author);

         await Task.CompletedTask;
      }

      public async Task DeleteAuthorAsync(Author author)
      {
         Delete(author);

         await Task.CompletedTask;
      }

   }
}