using LibraryApi.Identity.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Identity.Infrastructure.Data.Configuration
{
   /// <summary>
   /// Class to configure user entity in database
   /// </summary>
   public class UsersConfiguration : IEntityTypeConfiguration<User>
   {
      private const int EMAIL_MAX_LENGTH = 40;
      private const int NAME_MAX_LENGTH = 20;

      public void Configure(EntityTypeBuilder<User> builder)
      {
         builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(NAME_MAX_LENGTH);

         builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(NAME_MAX_LENGTH);


         builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(EMAIL_MAX_LENGTH);
      }
   }
}