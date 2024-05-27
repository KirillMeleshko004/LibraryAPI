using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
   public class RepositoryContext : DbContext
   {
      public DbSet<Book> Books { get; set; }
      public DbSet<Author> Authors { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         base.OnConfiguring(optionsBuilder);
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.ApplyConfiguration(new AuthorConfiguration());
         modelBuilder.ApplyConfiguration(new BooksConfiguration());
      }

      public RepositoryContext(DbContextOptions<RepositoryContext> options) 
         : base(options)
      {
         Database.EnsureCreated();
      }
   }
}