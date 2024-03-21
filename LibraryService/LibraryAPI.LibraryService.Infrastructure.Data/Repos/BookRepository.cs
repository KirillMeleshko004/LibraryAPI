using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;
using LibraryAPI.LibraryService.Infrastructure.Data.Extensions;
using LibraryAPI.LibraryService.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Repos
{
    /// <summary>
    /// Repository to manipulate books in data storage
    /// </summary>
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters, 
            bool trackChanges)
        {
            return await Get(trackChanges)
                .FilterBooks(parameters)
                .SortBooks(parameters.OrderBy)
                .ToListAsync();
        }
        public async Task<Book> GetBookByIdAsync(Guid id, bool trackChanges)
        {
            return await Get(b => b.Id.Equals(id), trackChanges)
                .SingleAsync();
        }
        public async Task<Book> GetBookByISBNAsync(string ISBN, bool trackChanges)
        {
            return await Get(b => b.ISBN.Equals(ISBN), trackChanges)
                .SingleAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            Create(book);

            await Task.CompletedTask;
            return;
        }
        public async Task UpdateBookAsync(Book book)
        {
            Update(book);
            
            await Task.CompletedTask;
            return;
        }
        public async Task DeleteBookAsync(Book book)
        {
            Delete(book);
            
            await Task.CompletedTask;
            return;
        }
    }
}