using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Identity.Shared.Dtos
{
   public record UserForAuthorizationDto
   {

      [Required(ErrorMessage = "Email is required field.")]
      [EmailAddress]
      public string Email { get; set; } = null!;
   
      [Required(ErrorMessage = "Password is required field.")]
      public string Password { get; set; } = null!;

   }
}