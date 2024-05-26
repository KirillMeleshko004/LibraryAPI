using Library.Domain.Entities;

namespace Library.UseCases.Common.Interfaces
{
   public interface IBookRepository
   {

      Task<IEnumerable<Book>> GetBooksAsync();
      Task<Book?> GetBookByIdAsync(Guid id);
      Task<Book?> GetBookByISBNAsync(string ISBN);

      Task AddBookAsync(Book book);
      Task UpdateBookAsync(Guid id, Book book);
      Task DeleteBookAsync(Book book);

   }
}