using System.ComponentModel.DataAnnotations;
using Identity.Domain.Enum;

namespace Identity.UseCases.Users.Dtos
{
   public record UserForCreationDto
   {

      [Required(ErrorMessage = "Email is required field.")]
      [EmailAddress]
      [MaxLength(40, ErrorMessage = "Email max length is 40.")]
      public string Email { get; set; } = null!;

      [Required(ErrorMessage = "Password is required field.")]
      [MaxLength(20, ErrorMessage = "Password max length is 20.")]
      public string Password { get; set; } = null!;

      [Required(ErrorMessage = "FirstName is required field.")]
      [MaxLength(20, ErrorMessage = "FirstName max length is 20.")]
      public string FirstName { get; set; } = null!;

      [Required(ErrorMessage = "LastName is required field.")]
      [MaxLength(20, ErrorMessage = "LastName max length is 20.")]
      public string LastName { get; set; } = null!;

      private readonly List<string> _roles = [];

      public List<string> UserRoles
      {
         get
         {
            if (_roles.Count == 0) _roles.Add(Roles.Customer.ToString());

            return _roles;
         }
         set
         {
            foreach (var role in value)
            {
               if (Enum.IsDefined(typeof(Roles), role))
               {
                  _roles.Add(role);
               }
            }
         }
      }

   }
}