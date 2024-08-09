using Identity.Domain.Entities;
using Identity.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configuration
{
   /// <summary>
   /// Class to configure role entity in database
   /// </summary>
   public class RolesConfiguration : IEntityTypeConfiguration<Role>
   {
      public void Configure(EntityTypeBuilder<Role> builder)
      {
         var roles = new List<Role>();

         var admin = new Role()
         {
            Id = new Guid("ca0b4257-5277-46e3-be6a-267319420ac4"),
            Name = "Admin",
            NormalizedName = "ADMIN"
         };

         var customer = new Role()
         {
            Id = new Guid("cca6b1ef-9373-4b47-9d7e-56c3ac31fb0d"),
            Name = "Customer",
            NormalizedName = "CUSTOMER"
         };

         roles.Add(admin);
         roles.Add(customer);

         builder.HasData(roles);
      }
   }
}