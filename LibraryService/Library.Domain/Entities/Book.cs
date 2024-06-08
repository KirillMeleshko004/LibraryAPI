namespace Library.Domain.Entities
{
    /// <summary>
    /// Class represents book entity
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; }

        public string ISBN { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public string AuthorName { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }


        public bool IsAvailable { get; set; } = true;

        public Guid? CurrentReaderId { get; set; }
        public Reader? CurrentReader { get; set; }

        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }

        public string? ImagePath { get; set; }
    }
}