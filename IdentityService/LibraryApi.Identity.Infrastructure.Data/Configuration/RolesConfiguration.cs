using LibraryApi.Identity.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Core.Enum;

namespace LibraryApi.Identity.Infrastructure.Data.Configuration
{
   /// <summary>
   /// Class to configure role entity in database
   /// </summary>
   public class RolesConfiguration : IEntityTypeConfiguration<Role>
   {
      public void Configure(EntityTypeBuilder<Role> builder)
      {
         var roles = new List<Role>();

         //Retrieving all available roles from enum
         foreach (var role in Enum.GetValues<Roles>())
         {
            roles.Add(new Role()
            {
               Id = Guid.NewGuid(),
               Name = role.ToString(),
               NormalizedName = role.ToString().ToUpper()
            });
         }

         builder.HasData(roles);
      }
   }
}