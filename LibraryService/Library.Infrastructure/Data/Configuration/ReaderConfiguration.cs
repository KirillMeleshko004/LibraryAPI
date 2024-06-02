using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data
{
   public class ReaderConfiguration : IEntityTypeConfiguration<Reader>
   {
      private const int EMAIL_MAX_LENGTH = 40;
      public void Configure(EntityTypeBuilder<Reader> builder)
      {
         builder.HasKey(r => r.Id);
         builder.HasAlternateKey(r => r.Email);

         builder.HasMany<Book>()
            .WithOne()
            .HasForeignKey(b => b.CurrentReaderEmail)
            .HasPrincipalKey(r => r.Email);

         builder.Property(r => r.Email)
            .HasMaxLength(EMAIL_MAX_LENGTH)
            .IsRequired();
      }
   }
}