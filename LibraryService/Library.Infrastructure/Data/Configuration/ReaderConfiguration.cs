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
         builder.HasIndex(r => r.Email)
            .IsUnique();

         builder.Property(r => r.Email)
            .HasMaxLength(EMAIL_MAX_LENGTH)
            .IsRequired();

         builder.HasMany<Book>()
            .WithOne()
            .HasForeignKey(b => b.CurrentReaderId)
            .OnDelete(DeleteBehavior.SetNull);
      }
   }
}