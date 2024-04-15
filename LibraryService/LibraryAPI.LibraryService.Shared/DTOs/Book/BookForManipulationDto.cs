using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Shared.DTOs
{
    public abstract record BookForManipulationDto
    {
        [Required]
        [RegularExpression(@"ISBN(?:-13)?:?\x20*(?=.{17}$)97(?:8|9)([ -])\d{1,5}\1\d{1,7}\1\d{1,6}\1\d$")]
        public string ISBN { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public string Genre { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}