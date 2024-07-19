using System.ComponentModel.DataAnnotations;
using Identity.Domain.Enum;

namespace Identity.UseCases.Users.Dtos
{
   public record UserForCreationDto
   {

      public string Email { get; set; } = null!;
      public string UserName { get; set; } = null!;

      public string Password { get; set; } = null!;

      public string FirstName { get; set; } = null!;

      public string LastName { get; set; } = null!;


      public List<string> UserRoles { get; set; } = null!;

   }
}