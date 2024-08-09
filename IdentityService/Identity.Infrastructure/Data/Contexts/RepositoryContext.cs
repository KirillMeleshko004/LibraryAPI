using Identity.Domain.Entities;
using Identity.Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Contexts
{
   public class RepositoryContext : IdentityDbContext<User, Role, Guid>
   {

      public RepositoryContext(DbContextOptions<RepositoryContext> options) :
         base(options)
      {
         if (Database.GetPendingMigrations().Any())
         {
            Database.Migrate();
         }
      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         base.OnConfiguring(optionsBuilder);
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         builder.ApplyConfiguration(new RolesConfiguration());
         builder.ApplyConfiguration(new UsersConfiguration());

         #region Seed test user

         var testUserId = new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e");

         var testUser = new User
         {
            Id = testUserId,
            Email = "testuser@gmail.com",
            EmailConfirmed = true,
            FirstName = "Kirill",
            LastName = "Meleshko",
            UserName = "kirilll",
            NormalizedUserName = "KIRILLL"
         };

         PasswordHasher<User> ph = new PasswordHasher<User>();
         testUser.PasswordHash = ph.HashPassword(testUser, "1234567890aB");


         builder.Entity<User>().HasData(testUser);


         builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
         {
            RoleId = new Guid("ca0b4257-5277-46e3-be6a-267319420ac4"),
            UserId = testUserId
         },
         new IdentityUserRole<Guid>
         {
            RoleId = new Guid("cca6b1ef-9373-4b47-9d7e-56c3ac31fb0d"),
            UserId = testUserId
         });

         #endregion

         base.OnModelCreating(builder);
      }

   }
}