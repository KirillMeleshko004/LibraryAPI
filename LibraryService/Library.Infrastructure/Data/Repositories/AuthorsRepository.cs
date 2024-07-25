using Library.Domain.Entities;
using Library.UseCases.Common.Interfaces;

namespace Library.Infrastructure.Data
{
   /// <summary>
   /// Repository to manipulate authors in data storage
   /// </summary>
   public class AuthorsRepository : BaseRepository<Author>, IAuthorRepository
   {
      public AuthorsRepository(RepositoryContext context) : base(context) { }
   }
}