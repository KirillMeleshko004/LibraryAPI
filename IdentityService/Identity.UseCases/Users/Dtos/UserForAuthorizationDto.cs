using System.ComponentModel.DataAnnotations;

namespace Identity.UseCases.Users.Dtos
{
   public record UserForAuthorizationDto
   {

      [Required(ErrorMessage = "Email is required field.")]
      [EmailAddress]
      [MaxLength(40, ErrorMessage = "Email max length is 40.")]
      public string Email { get; set; } = null!;

      [Required(ErrorMessage = "Password is required field.")]
      [MaxLength(20, ErrorMessage = "Password max length is 20.")]
      public string Password { get; set; } = null!;

   }
}