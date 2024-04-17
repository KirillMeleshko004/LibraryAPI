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
        private const int GENRE_MAX_LENGTH = 30;

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
                    Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"),
                    Title = "War and Peace",
                    AuthorId = new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"),
                    ISBN = "ISBN 13: 9781566190275",
                    Genre = "Novel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("2f346383-bd6a-4564-8dce-343c355e795a"),
                    Title = "Anna Karenina",
                    AuthorId = new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"),
                    ISBN = "ISBN 13: 9780672523830",
                    Genre = "Novel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("ad9c4dbe-5dff-43e0-a58c-cea9327a4464"),
                    Title = "Old Izergil",
                    AuthorId = new Guid("21cb29da-047a-4d85-a581-8ef6cffec67f"),
                    ISBN = "ISBN-13: 9798390533352",
                    Genre = "Friction",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("f31001c4-fb5d-42f0-aafd-dd0e6e08476e"),
                    Title = "Ascendance of a bookworm",
                    AuthorId = new Guid("3bc8f089-2d00-4346-af71-d9f9fcdceb20"),
                    ISBN = "ISBN-13: 9781718357976",
                    Genre = "Friction",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("6328fcf9-5846-4f7c-960c-da5ea5c32f22"),
                    Title = "Crime and Punishment",
                    AuthorId = new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"),
                    ISBN = "ISBN 13: 9785050000149",
                    Genre = "Novel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("424e64e8-c811-42ef-8153-f7952ced8c51"),
                    Title = "The Brothers Karamazov",
                    AuthorId = new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"),
                    ISBN = "ISBN 10: 0374528373",
                    Genre = "Novel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("81ebde25-7b81-4bf2-8691-edef624642d8"),
                    Title = "Dead souls",
                    AuthorId = new Guid("ec891ac2-f620-415f-9f86-3d15259eb071"),
                    ISBN = "ISBN 10: 0300060998",
                    Genre = "Satire",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("8e32b21e-1a32-4272-bc46-6f7b709a7696"),
                    Title = "Romeo and Juliet",
                    AuthorId = new Guid("4792ce31-a3e8-4df3-b0d7-4ea1c8e40dbd"),
                    ISBN = "ISBN 13: 9780671722852",
                    Genre = "Shakespearean tragedy",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                },
                new Book
                {
                    Id = new Guid("a0283873-60b8-45de-a411-02a0a3fbc465"),
                    Title = "Alpine Ballad",
                    AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                    ISBN = "ISBN 13: 9781909156821",
                    Genre = "War novel",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed."
                }
            );
        }
    }
}