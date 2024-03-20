using LibraryAPI.LibraryService.Domain.Core;
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
            base.OnModelCreating(modelBuilder);
        }
    }
}