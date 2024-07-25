using System.Linq.Expressions;
using Library.UseCases.Common.RequestFeatures;

namespace Library.UseCases.Common.Interfaces
{
   public interface IBaseRepository<T>
   {
      public Task<T?> GetSingle(Expression<Func<T, bool>> condition,
         Expression<Func<T, object>>? include = null,
         CancellationToken cancellationToken = default);
      public Task<IPagedEnumerable<T>> GetRange(RequestParameters parameters,
         Expression<Func<T, bool>>? condition = null,
         Expression<Func<T, object>>? include = null,
         CancellationToken cancellationToken = default);
      public void Create(T entity, CancellationToken cancellationToken);
      public void Update(T entity, CancellationToken cancellationToken);
      public void Delete(T entity, CancellationToken cancellationToken);
   }
}