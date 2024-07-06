using Identity.Domain.Entities;
using Identity.Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Contexts
{
   public class RepositoryContext : IdentityDbContext<User, Role, Guid>
   {

      public RepositoryContext(DbContextOptions<RepositoryContext> options) :
         base(options)
      {
         Database.EnsureCreated();
      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         base.OnConfiguring(optionsBuilder);
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         builder.ApplyConfiguration(new RolesConfiguration());
         builder.ApplyConfiguration(new UsersConfiguration());

         base.OnModelCreating(builder);
      }

   }
}