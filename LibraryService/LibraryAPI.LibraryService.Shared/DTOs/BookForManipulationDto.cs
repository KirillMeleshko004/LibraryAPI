using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Shared.DTOs
{
    public abstract record BookForManipulationDto
    {
        [Required]
        public string ISBN { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;
        
        [Required]
        public string Description { get; set; } = null!;
        
        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}