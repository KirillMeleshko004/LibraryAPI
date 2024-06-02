using Library.Domain.Entities;

namespace Library.UseCases.Common.Interfaces
{
   public interface IReaderRepository
   {
      Task<Reader?> GetReaderByEmailAsync(string email, CancellationToken cancellationToken);
      Task AddReaderAsync(string email, CancellationToken cancellationToken);
   }
}