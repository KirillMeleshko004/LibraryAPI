using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.LibraryService.Shared.DTOs
{
   public abstract record AuthorForManipulationDto
   {

      [Required(ErrorMessage = "FirstName field is required")]
      [MaxLength(20, ErrorMessage = "Max firstname length is 20.")]
      public string FirstName { get; set; } = null!;

      [Required(ErrorMessage = "LastName field is required")]
      [MaxLength(20, ErrorMessage = "Max lastname length is 20.")]
      public string LastName { get; set; } = null!;
   }
}