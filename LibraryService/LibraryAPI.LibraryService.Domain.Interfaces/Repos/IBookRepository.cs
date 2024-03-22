using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Domain.Interfaces.Repos
{
    public interface IBookRepository
    {
        
        Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters, bool trackChanges);
        Task<Book> GetBookByIdAsync(Guid id, bool trackChanges);
        Task<Book> GetBookByISBNAsync(string ISBN, bool trackChanges);

        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Book book);

    }
}