using System.Linq.Expressions;
using Library.Domain.Entities;
using Library.Infrastructure.Data.Extensions;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.Utility;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
   /// <summary>
   /// Base class for all repositories
   /// Include basic CRUD operations
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class BaseRepository<T> : IBaseRepository<T> where T : Entity
   {
      private protected readonly RepositoryContext _context;

      public BaseRepository(RepositoryContext context)
      {
         _context = context;
      }

      public async Task<T?> GetSingle(Expression<Func<T, bool>> condition,
         Expression<Func<T, object>>? include = null,
         CancellationToken cancellationToken = default)
      {
         var query = _context.Set<T>()
            .Where(condition)
            .AsNoTracking();

         if (include != null)
         {
            query = query.Include(include);
         }

         return await query
            .SingleOrDefaultAsync(cancellationToken);
      }

      public async Task<IPagedEnumerable<T>> GetRange(RequestParameters parameters,
         Expression<Func<T, bool>>? condition,
         Expression<Func<T, object>>? include = null,
         CancellationToken cancellationToken = default)
      {
         var query = _context.Set<T>()
            .AsNoTracking();

         if (condition != null)
         {
            query = query.Where(condition);
         }

         query = query
            .Filter(parameters.Filters)
            .Search(parameters.SearchTerm);

         var count = query.Count();

         query = query
            .Sort(parameters.OrderBy)
            .Page(parameters);

         if (include != null)
         {
            query = query.Include(include);
         }

         var result = await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);

         return new PagedList<T>(result, parameters.PageSize,
            parameters.PageNumber, count);
      }

      public void Create(T entity, CancellationToken cancellationToken)
      {
         _context.Set<T>().Add(entity);
      }

      public void Update(T entity, CancellationToken cancellationToken)
      {
         _context.Set<T>().Update(entity);
      }

      public void Delete(T entity, CancellationToken cancellationToken)
      {
         _context.Set<T>().Remove(entity);
      }

   }
}