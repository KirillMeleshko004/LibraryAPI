namespace Library.Domain.Entities
{
   public class Reader
   {
      public Guid Id { get; set; }
      public string Email { get; set; } = null!;

      public IEnumerable<Book>? BorrowedBooks { get; set; }
   }
}