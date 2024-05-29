using System.ComponentModel.DataAnnotations;

namespace Library.UseCases.Authors.DTOs
{
   /// <summary>
   /// Base Dto for authors send from user
   /// </summary>
   public abstract record AuthorForManipulationDto
   {

      [Required(ErrorMessage = "FirstName field is required")]
      [MaxLength(20, ErrorMessage = "Max firstname length is 20.")]
      public string FirstName { get; set; } = null!;

      [Required(ErrorMessage = "LastName field is required")]
      [MaxLength(20, ErrorMessage = "Max lastname length is 20.")]
      public string LastName { get; set; } = null!;
   
      [Required(ErrorMessage = "DateOfBirth field is required")]
      public DateOnly? DateOfBirth { get; set; }

      [Required(ErrorMessage = "Country field is required")]
      [MaxLength(62, ErrorMessage = "Max country length is 62.")]
      public string Country { get; set; } = null!;

   }
}