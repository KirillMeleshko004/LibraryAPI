using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Domain.Interfaces.Repos
{
    public interface IAuthorRepository
    {

        Task<IEnumerable<Author>> GetAuthorsAsync(AuthorParameters parameters, bool trackChanges);
        Task<Author?> GetAuthorByIdAsync(Guid id, bool trackChanges);

        Task AddAuthorAsync(Author author);
        Task DeleteAuthorAsync(Author author);

    }
}