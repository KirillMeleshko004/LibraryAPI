using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Identity.Infrastructure.Data.Contexts
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

         base.OnModelCreating(builder);
      }

    }
}