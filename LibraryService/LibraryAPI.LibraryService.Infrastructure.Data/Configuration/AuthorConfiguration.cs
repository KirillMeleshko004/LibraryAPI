using LibraryAPI.LibraryService.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Configuration
{
   public class AuthorConfiguration : IEntityTypeConfiguration<Author>
   {
      private const int NAME_MAX_LENGTH = 20;

      public void Configure(EntityTypeBuilder<Author> builder)
      {
         builder.HasKey(b => b.Id);

         builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(NAME_MAX_LENGTH);

         builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(NAME_MAX_LENGTH);

         //Configure one to many relationship between author and books
         builder.HasMany(a => a.Books)
               .WithOne(b => b.Author)
               .HasForeignKey(b => b.AuthorId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);


         builder.HasData(
            new Author
            {
               Id = new Guid("72EB1467-698E-4566-999F-08DC03C404E3"),
               FirstName = "Test First Name",
               LastName = "Test Last Name"
            }
         );
      }
   }
}