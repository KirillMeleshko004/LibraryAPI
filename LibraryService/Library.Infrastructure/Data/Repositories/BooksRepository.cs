using Library.Domain.Entities;
using Library.UseCases.Common.Interfaces;

namespace Library.Infrastructure.Data
{
   /// <summary>
   /// Repository to manipulate books in data storage
   /// </summary>
   public class BooksRepository : BaseRepository<Book>, IBookRepository
   {
      public BooksRepository(RepositoryContext context) : base(context) { }
   }
}