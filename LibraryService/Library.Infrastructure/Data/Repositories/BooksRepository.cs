using Library.Domain.Entities;
using Library.Infrastructure.Data.Extensions;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
   /// <summary>
   /// Repository to manipulate books in data storage
   /// </summary>
   public class BooksRepository : RepositoryBase<Book>, IBookRepository
   {
      public BooksRepository(RepositoryContext context) : base(context) {}

      public async Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters,
         CancellationToken cancellationToken)
      {
         return await Get()
            .FilterBooks(parameters)
            .Sort(parameters.OrderBy)
            .Page(parameters)
            .Include(b => b.Author)
            .ToListAsync(cancellationToken);
      }
      public async Task<Book?> GetBookByIdAsync(Guid id,
         CancellationToken cancellationToken)
      {
         return await Get(b => b.Id.Equals(id))
            .Include(b => b.Author)
            .SingleOrDefaultAsync(cancellationToken);
      }
      public async Task<Book?> GetBookByISBNAsync(string ISBN,
         CancellationToken cancellationToken)
      {
         return await Get(b => b.ISBN.Equals(ISBN))
            .Include(b => b.Author)
            .SingleOrDefaultAsync(cancellationToken);
      }

      public async Task AddBookAsync(Book book,
         CancellationToken cancellationToken)
      {
         Create(book);

         //Loading related entity to created book
         await _context.Entry(book).Reference(b => b.Author).LoadAsync(cancellationToken);
      }

      public async Task DeleteBookAsync(Book book,
         CancellationToken cancellationToken)
      {
         Delete(book);
         await Task.CompletedTask;
      }

      public async Task UpdateBookAsync(Book book,
         CancellationToken cancellationToken)
      {
         Update(book);
         await Task.CompletedTask;
      }
   }
}