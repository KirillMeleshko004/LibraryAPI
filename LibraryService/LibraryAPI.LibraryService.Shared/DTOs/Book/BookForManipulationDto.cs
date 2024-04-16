using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Shared.DTOs
{
    public abstract record BookForManipulationDto
    {
        [Required(ErrorMessage = "ISBN field is required")]
        [RegularExpression(@"ISBN(?:-13)?:?\x20*(?=.{17}$)97(?:8|9)([ -])\d{1,5}\1\d{1,7}\1\d{1,6}\1\d$")]
        public string ISBN { get; set; } = null!;

        [Required(ErrorMessage = "Title field is required")]
        [MaxLength(40, ErrorMessage = "Max title length is 40.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "AuthorId field is required")]
        public Guid? AuthorId { get; set; }

        [Required(ErrorMessage = "Genre field is required")]
        [MaxLength(15, ErrorMessage = "Max genre length is 15.")]
        public string Genre { get; set; } = null!;

        [Required(ErrorMessage = "Description field is required")]
        [MaxLength(300, ErrorMessage = "Max description length is 300.")]
        public string Description { get; set; } = null!;

        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}