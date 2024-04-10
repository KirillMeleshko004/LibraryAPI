using System.ComponentModel.DataAnnotations;
using Users.Domain.Core.Enum;

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

      private readonly List<string> _roles = [];
      
      public List<string> UserRoles 
      { 
         get
         {
            if(_roles.Count == 0) _roles.Add(Roles.Customer.ToString());

            return _roles;
         }
         set
         {
            foreach(var role in value)
            {
               if(Enum.IsDefined(typeof(Roles), role))
               {
                  _roles.Add(role);
               }
            }
         }
      }

   }
}