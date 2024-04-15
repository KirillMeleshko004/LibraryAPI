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

        private const int TITLE_MAX_LENGTH = 40;
        private const int DESCRIPTION_MAX_LENGTH = 300;
        private const int GENRE_MAX_LENGTH = 15;

        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .HasMaxLength(TITLE_MAX_LENGTH)
                .IsRequired();

            builder.Property(b => b.ISBN)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(DESCRIPTION_MAX_LENGTH);

            builder.Property(b => b.Genre)
                .HasMaxLength(GENRE_MAX_LENGTH);

            //Set initial data
            builder.HasData(
                new Book
                {
                    Id = new Guid("72EB1467-698E-4566-999F-08DC03C404E3"),
                    Title = "Test Book",
                    AuthorId = new Guid("72EB1467-698E-4566-999F-08DC03C404E3"),
                    ISBN = "ISBN-13 978-1-4028-9462-6",
                    Genre = "Test Genre",
                    Description = "Test description"
                }
            );
        }
    }
}