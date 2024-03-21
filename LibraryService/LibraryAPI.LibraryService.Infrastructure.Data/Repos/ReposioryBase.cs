using System.Linq.Expressions;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Repos
{
    /// <summary>
    /// Base class for all repositories
    /// Include basic CRUD operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get(bool trackChanges)
        {
            if(trackChanges)
            {
                return _context.Set<T>();
            }
            else
            {
                return _context.Set<T>().AsNoTracking();
            }
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter, bool trackChanges)
        {
            if(trackChanges)
            {
                return _context.Set<T>()
                    .Where(filter);
            }
            else
            {
                return _context.Set<T>()
                    .Where(filter)
                    .AsNoTracking();
            }
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}