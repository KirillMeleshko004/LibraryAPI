using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Api.Controllers.ViewModels
{
    public record BookForCreationViewModel
    {
        [Required(ErrorMessage = "ISBN field is required")]
        [RegularExpression(@"(^(ISBN|ISBN(-|\s)?10)?:?\s*\d{10}$|(^(ISBN|ISBN(-|\s)?13)?:?\s*\d{13}$))")]
        public string ISBN { get; set; } = null!;

        [Required(ErrorMessage = "Title field is required")]
        [MaxLength(40, ErrorMessage = "Max title length is 40.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "AuthorId field is required")]
        public Guid AuthorId { get; set; }

        [Required(ErrorMessage = "Genre field is required")]
        [MaxLength(30, ErrorMessage = "Max genre length is 30.")]
        public string Genre { get; set; } = null!;

        [Required(ErrorMessage = "Description field is required")]
        [MaxLength(300, ErrorMessage = "Max description length is 300.")]
        public string Description { get; set; } = null!;

        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }

        public IFormFile? Image { get; set; }
    }
}