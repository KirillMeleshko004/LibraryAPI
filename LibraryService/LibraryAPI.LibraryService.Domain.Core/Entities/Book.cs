using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Domain.Core.Entities
{
    public class Book
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        //apply validation
        public string ISBN { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public DateTime? BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }
    }
}