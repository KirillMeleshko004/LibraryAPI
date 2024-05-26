using LibraryAPI.LibraryService.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Configuration
{
   public class AuthorConfiguration : IEntityTypeConfiguration<Author>
   {
      private const int NAME_MAX_LENGTH = 20;
      private const int COUNTRY_MAX_LENGTH = 62;

      public void Configure(EntityTypeBuilder<Author> builder)
      {
         builder.HasKey(b => b.Id);

         builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(NAME_MAX_LENGTH);

         builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(NAME_MAX_LENGTH);

         builder.Property(a => a.DateOfBirth)
            .IsRequired();

         builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(COUNTRY_MAX_LENGTH);

         //Configure one to many relationship between author and books
         builder.HasMany(a => a.Books)
               .WithOne(b => b.Author)
               .HasForeignKey(b => b.AuthorId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);


         //Set initial data
         builder.HasData(
            new Author
            {
               Id = new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"),
               FirstName = "Lev",
               LastName = "Tolstoy",
               DateOfBirth = new DateOnly(1828, 9, 9),
               Country = "Russia"
            },
            new Author
            {
               Id = new Guid("21cb29da-047a-4d85-a581-8ef6cffec67f"),
               FirstName = "Maxim",
               LastName = "Gorkiy",
               DateOfBirth = new DateOnly(1868, 3, 28),
               Country = "Russia"
            },
            new Author
            {
               Id = new Guid("3bc8f089-2d00-4346-af71-d9f9fcdceb20"),
               FirstName = "Miya",
               LastName = "Kazuki",
               DateOfBirth = new DateOnly(1995, 7, 3),
               Country = "Japan"
            },
            new Author
            {
               Id = new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"),
               FirstName = "Fedor",
               LastName = "Dostoyevskiy",
               DateOfBirth = new DateOnly(1821, 11, 11),
               Country = "Russia"
            },
            new Author
            {
               Id = new Guid("ec891ac2-f620-415f-9f86-3d15259eb071"),
               FirstName = "Nikolay",
               LastName = "Gogol",
               DateOfBirth = new DateOnly(1809, 1, 4),
               Country = "Russia"
            },
            new Author
            {
               Id = new Guid("4792ce31-a3e8-4df3-b0d7-4ea1c8e40dbd"),
               FirstName = "William",
               LastName = "Shakespeare",
               DateOfBirth = new DateOnly(1564, 4, 23),
               Country = "United Kingdom"
            },
            new Author
            {
               Id = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
               FirstName = "Vasil",
               LastName = "Bykov",
               DateOfBirth = new DateOnly(1924, 6, 22),
               Country = "Belarus"
            }
         );
      }
   }
}