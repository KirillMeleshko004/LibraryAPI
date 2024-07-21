using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configuration
{
   /// <summary>
   /// Class to configure author entity in database
   /// </summary>
   public class AuthorConfiguration : BaseEntityConfiguration<Author>
   {
      private const int NAME_MAX_LENGTH = 20;
      private const int COUNTRY_MAX_LENGTH = 62;

      public override void Configure(EntityTypeBuilder<Author> builder)
      {
         base.Configure(builder);

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

      }
   }
}