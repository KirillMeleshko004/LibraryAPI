using Library.Domain.Entities;
using Library.UseCases.Common.Interfaces;

namespace Library.Infrastructure.Data
{
   public class ReaderRepository : BaseRepository<Reader>, IReaderRepository
   {
      public ReaderRepository(RepositoryContext context) : base(context) { }
   }
}