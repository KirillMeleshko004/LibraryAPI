using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities
{
   /// <summary>
   /// Class represents user entity in DB
   /// </summary>
   public class User : IdentityUser<Guid>
   {
      public string FirstName { get; set; } = null!;

      public string LastName { get; set; } = null!;

      public string? RefreshToken { get; set; }
      public DateTime RefreshTokenExpiryTime { get; set; }
   }
}