using System.ComponentModel.DataAnnotations;

namespace Identity.UseCases.Common.Tokens
{
   public record Token
   {

      [Required(ErrorMessage = "AccessToken is required field.")]
      public string AccessToken { get; set; } = null!;

      [Required(ErrorMessage = "RefreshToken is required field.")]
      public string RefreshToken { get; set; } = null!;

   }
}