using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraryApi.Identity.Domain.Core.Entities
{
   public class User : IdentityUser<Guid>
   {
      [Required]
      public string FirstName { get; set; } = null!;

      [Required]
      public string LastName { get; set; } = null!;
   }
}