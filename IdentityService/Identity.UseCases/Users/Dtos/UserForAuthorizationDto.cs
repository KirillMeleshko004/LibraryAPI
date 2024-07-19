using System.ComponentModel.DataAnnotations;

namespace Identity.UseCases.Users.Dtos
{
   public record UserForAuthorizationDto
   {
      public string UserName { get; set; } = null!;
      public string Password { get; set; } = null!;

   }
}