using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
      public BooksRepository(RepositoryContext context) : base(context) { }

      public async Task<IEnumerable<Book>> GetBooksAsync(
         BookParameters parameters,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include)
      {
         var query = Get()
            .FilterBooks(parameters)
            .SearchByName(parameters.SearchTerm)
            .Sort(parameters.OrderBy)
            .Page(parameters);

         if (include != null)
         {
            query = query.Include(include);
         }

         return await query
            .ToListAsync(cancellationToken);
      }

      public async Task<IEnumerable<Book>> GetBookByAuthorAsync(
         BookParameters parameters,
         Guid authorId,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include)
      {
         var query = Get()
            .Where(b => b.AuthorId.Equals(authorId))
            .FilterBooks(parameters)
            .Sort(parameters.OrderBy)
            .Page(parameters);

         if (include != null)
         {
            query = query.Include(include);
         }

         return await query
            .ToListAsync(cancellationToken);
      }

      public async Task<IEnumerable<Book>> GetBookByReaderAsync(Guid readerId,
         CancellationToken cancellationToken, Expression<Func<Book, object>>? include = null)
      {
         var books = await Get()
            .Where(b => b.CurrentReaderId.Equals(readerId))
            .ToListAsync(cancellationToken);

         return books;
      }

      public async Task<Book?> GetBookByIdAsync(Guid id,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include)
      {
         var query = Get(b => b.Id.Equals(id));

         if (include != null)
         {
            query = query.Include(include);
         }

         return await query
            .SingleOrDefaultAsync(cancellationToken);
      }
      public async Task<Book?> GetBookByISBNAsync(string ISBN,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include)
      {
         var query = Get(b => b.ISBN.Equals(ISBN));

         if (include != null)
         {
            query = query.Include(include);
         }

         return await query
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