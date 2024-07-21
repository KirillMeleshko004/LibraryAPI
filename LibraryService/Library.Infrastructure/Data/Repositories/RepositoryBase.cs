using System.Linq.Expressions;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
   /// <summary>
   /// Base class for all repositories
   /// Include basic CRUD operations
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class RepositoryBase<T> where T : Entity
   {
      private protected readonly RepositoryContext _context;

      public RepositoryBase(RepositoryContext context)
      {
         _context = context;
      }

      public IQueryable<T> Get()
      {
         return _context.Set<T>()
            .AsNoTracking();
      }

      public IQueryable<T> Get(Expression<Func<T, bool>> filter)
      {
         return _context.Set<T>()
            .Where(filter)
            .AsNoTracking();
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