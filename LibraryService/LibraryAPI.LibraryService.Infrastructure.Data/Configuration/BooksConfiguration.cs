using LibraryAPI.LibraryService.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Configuration
{
    /// <summary>
    /// Class to configure book entity in database
    /// </summary>
    public class BooksConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            //Set initial data
            builder.HasData(
                new Book
                {
                    Id = new Guid("72EB1467-698E-4566-999F-08DC03C404E3"),
                    Title = "Test Book",
                    Author = "Test Author",
                    ISBN = "978-0-596-52068-7",
                    Genre = "Test Genre",
                    Description = "Test description"
                }
            );
        }
    }
}