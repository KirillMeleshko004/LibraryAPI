using System.Linq.Expressions;
using Library.Domain.Entities;
using Library.UseCases.Common.RequestFeatures;

namespace Library.UseCases.Common.Interfaces
{
   public interface IBookRepository
   {

      Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include = null);
      Task<IEnumerable<Book>> GetBookByAuthorAsync(BookParameters parameters,
         Guid authorId,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include = null);
      Task<IEnumerable<Book>> GetBookByReaderAsync(Guid readerId,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include = null);

      Task<Book?> GetBookByIdAsync(Guid id,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include = null);
      Task<Book?> GetBookByISBNAsync(string ISBN,
         CancellationToken cancellationToken,
         Expression<Func<Book, object>>? include = null);

      Task AddBookAsync(Book book, CancellationToken cancellationToken);
      Task UpdateBookAsync(Book book, CancellationToken cancellationToken);
      Task DeleteBookAsync(Book book, CancellationToken cancellationToken);

   }
}