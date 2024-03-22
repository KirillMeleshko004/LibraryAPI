using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Repos
{
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}