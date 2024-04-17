namespace LibraryAPI.LibraryService.Domain.Core.Entities
{
    /// <summary>
    /// Class represents book entity in DB
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; }

        //apply validation
        public string ISBN { get; set; } = null!;

        public string Title { get; set; } = null!;


        public string? Description { get; set; }

        public string? Genre { get; set; }

        public DateTime? BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }


        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}