using System.Linq.Expressions;

namespace LibraryAPI.LibraryService.Domain.Interfaces.Repos
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> Get(bool trackChanges);

        IQueryable<T> Get(Expression<Func<T, bool>> filter, bool trackChanges);

        void Create(T entity);
        
        void Delete(T entity);

        void Update(T entity);
    }
}