using System.ComponentModel.DataAnnotations;

namespace Identity.UseCases.Tokens.Dtos
{
   public record TokenDto
   {

      [Required(ErrorMessage = "AccessToken is required field.")]
      public string AccessToken { get; set; } = null!;

      [Required(ErrorMessage = "RefreshToken is required field.")]
      public string RefreshToken { get; set; } = null!;

   }
}