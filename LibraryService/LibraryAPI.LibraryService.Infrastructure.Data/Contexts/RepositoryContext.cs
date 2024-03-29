using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Contexts
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BooksConfiguration());
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}