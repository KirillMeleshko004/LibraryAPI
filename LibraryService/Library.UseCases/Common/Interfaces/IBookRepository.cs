using Library.Domain.Entities;
using Library.UseCases.Common.RequestFeatures;

namespace Library.UseCases.Common.Interfaces
{
   public interface IBookRepository
   {

      Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters, 
         CancellationToken cancellationToken);
      Task<Book?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken);
      Task<Book?> GetBookByISBNAsync(string ISBN, CancellationToken cancellationToken);

      Task AddBookAsync(Book book, CancellationToken cancellationToken);
      Task UpdateBookAsync(Book book, CancellationToken cancellationToken);
      Task DeleteBookAsync(Book book, CancellationToken cancellationToken);

   }
}