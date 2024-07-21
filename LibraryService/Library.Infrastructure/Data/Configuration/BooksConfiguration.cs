using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configuration
{
   /// <summary>
   /// Class to configure book entity in database
   /// </summary>
   public class BooksConfiguration : BaseEntityConfiguration<Book>
   {

      private const int TITLE_MAX_LENGTH = 40;
      private const int DESCRIPTION_MAX_LENGTH = 300;
      private const int GENRE_MAX_LENGTH = 30;
      private const int AUTHOR_NAME_MAX_LENGTH = 20;

      public override void Configure(EntityTypeBuilder<Book> builder)
      {
         base.Configure(builder);

         builder.Property(b => b.Title)
               .HasMaxLength(TITLE_MAX_LENGTH)
               .IsRequired();

         builder.Property(b => b.AuthorName)
               .HasMaxLength(AUTHOR_NAME_MAX_LENGTH)
               .IsRequired();

         builder.Property(b => b.ISBN)
               .IsRequired();

         builder.Property(b => b.Description)
               .HasMaxLength(DESCRIPTION_MAX_LENGTH);

         builder.Property(b => b.Genre)
               .HasMaxLength(GENRE_MAX_LENGTH);

         
      }
   }
}