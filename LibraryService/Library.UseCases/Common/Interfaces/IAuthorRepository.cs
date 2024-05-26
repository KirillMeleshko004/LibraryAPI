using Library.Domain.Entities;

namespace Library.UseCases.Common.Interfaces
{
   public interface IAuthorRepository
   {
      Task<IEnumerable<Author>> GetAuthorsAsync();
      Task<Author?> GetAuthorByIdAsync(Guid id);

      Task AddAuthorAsync(Author author);
      Task UpdateAuthorAsync(Guid id, Author author);
      Task DeleteAuthorAsync(Author author);

   }
}