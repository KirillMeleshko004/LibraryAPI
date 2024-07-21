namespace Library.Domain.Entities
{
   public class Reader : Entity
   {
      public string Email { get; set; } = null!;

      public IEnumerable<Book>? BorrowedBooks { get; set; }
   }
}