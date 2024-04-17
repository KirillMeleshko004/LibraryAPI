namespace LibraryAPI.LibraryService.Domain.Core.Entities
{
   /// <summary>
   /// Class represents author entity in DB
   /// </summary>
   public class Author
   {
      public Guid Id { get; set; }

      public string FirstName { get; set; } = null!;
      public string LastName { get; set; } = null!;

      public IEnumerable<Book>? Books { get; set; }
   }
}