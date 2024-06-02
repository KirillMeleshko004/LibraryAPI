namespace Library.UseCases.Common.Interfaces
{
   /// <summary>
   /// Represents unit of work pattern. 
   /// Ensures that all repositories use same data context
   /// </summary>
   public interface IRepositoryManager
   {
      IBookRepository Books { get; }

      IAuthorRepository Authors { get; }
      IReaderRepository Readers { get; }

      Task SaveChangesAsync();
   }
}