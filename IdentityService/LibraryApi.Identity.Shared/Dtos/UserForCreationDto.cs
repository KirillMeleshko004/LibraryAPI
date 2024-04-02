using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Identity.Shared.Dtos
{
   public record UserForCreationDto
   {

      [Required(ErrorMessage = "Email is required field.")]
      [EmailAddress]
      public string Email { get; set; } = null!;
   
      [Required(ErrorMessage = "Password is required field.")]
      public string Password { get; set; } = null!;

      [Required(ErrorMessage = "FirstName is required field.")]
      public string FirstName { get; set; } = null!;

      [Required(ErrorMessage = "LastName is required field.")]
      public string LastName { get; set; } = null!;

   }
}