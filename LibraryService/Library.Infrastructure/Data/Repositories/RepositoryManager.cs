using Library.Domain.Entities;
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
         _context.ChangeTracker.DetectChanges();
         var a = _context.ChangeTracker.Entries();
         var changed = _context.ChangeTracker.Entries()
            .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Modified ||
               e.State == Microsoft.EntityFrameworkCore.EntityState.Added);

         foreach (var entry in changed)
         {
            if (!typeof(Entity).IsAssignableFrom(entry.Entity.GetType()))
            {
               continue;
            }
            var entity = entry.Entity as Entity;

            if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
               entity!.CreatedAt = DateTime.Now;
            }

            entity!.ModifiedAt = DateTime.Now;
         }
         await _context.SaveChangesAsync();
      }
   }
}