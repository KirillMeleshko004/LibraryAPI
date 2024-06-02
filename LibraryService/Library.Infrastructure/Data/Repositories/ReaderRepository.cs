using Library.Domain.Entities;
using Library.UseCases.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
   public class ReaderRepository : RepositoryBase<Reader>, IReaderRepository
   {
      public ReaderRepository(RepositoryContext context) : base(context) { }

      public async Task<Reader?> GetReaderByEmailAsync(string email,
         CancellationToken cancellationToken)
      {
         return await Get(r => r.Email.Equals(email))
            .SingleOrDefaultAsync(cancellationToken);
      }

      public Task AddReaderAsync(string email, CancellationToken cancellationToken)
      {
         var reader = new Reader() { Email = email };

         Create(reader);

         return Task.CompletedTask;
      }
   }
}