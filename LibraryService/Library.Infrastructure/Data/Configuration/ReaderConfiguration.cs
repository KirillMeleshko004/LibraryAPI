using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configuration
{
   public class ReaderConfiguration : BaseEntityConfiguration<Reader>
   {
      private const int EMAIL_MAX_LENGTH = 40;
      public override void Configure(EntityTypeBuilder<Reader> builder)
      {
         base.Configure(builder);

         builder.HasIndex(r => r.Email)
            .IsUnique();

         builder.Property(r => r.Email)
            .HasMaxLength(EMAIL_MAX_LENGTH)
            .IsRequired();

         builder.HasMany<Book>()
            .WithOne(b => b.CurrentReader)
            .HasForeignKey(b => b.CurrentReaderId)
            .OnDelete(DeleteBehavior.SetNull);
      }
   }
}