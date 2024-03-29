using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Repos
{
    /// <summary>
    /// Class represents single unit of work for reposiories.
    /// RepositoryManager instance created once for each request and ensures
    /// that all repositories use same database context
    /// </summary>
    public class RepositoryManager : IRepositoryManager
    {   
        private readonly RepositoryContext _context;
        public IBookRepository Books 
        { 
            get 
            {
                return new BookRepository(_context);
            }
        }

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }

        //Saves all changes made during request to database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}