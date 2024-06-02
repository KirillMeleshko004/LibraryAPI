using Library.UseCases.Common.Interfaces;

namespace Library.Infrastructure.Data
{
   public class RepositoryManager : IRepositoryManager
   {
      private readonly Lazy<IBookRepository> _books;
      private readonly Lazy<IAuthorRepository> _authors;
      private readonly Lazy<IReaderRepository> _readers;

      private readonly RepositoryContext _context;
      public IBookRepository Books
      {
         get
         {
            return _books.Value;
         }
      }

      public IAuthorRepository Authors
      {
         get
         {
            return _authors.Value;
         }
      }

      public IReaderRepository Readers
      {
         get
         {
            return _readers.Value;
         }
      }

      public RepositoryManager(RepositoryContext context)
      {
         _context = context;

         _books = new Lazy<IBookRepository>(
            () => new BooksRepository(context)
         );

         _authors = new Lazy<IAuthorRepository>(
            () => new AuthorsRepository(context)
         );

         _readers = new Lazy<IReaderRepository>(
            () => new ReaderRepository(context)
         );
      }

      //Saves all changes made during request to database
      public async Task SaveChangesAsync()
      {
         await _context.SaveChangesAsync();
      }
   }
}