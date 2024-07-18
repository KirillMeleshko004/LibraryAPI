using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(62)", maxLength: 62, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CurrentReaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BorrowTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Readers_CurrentReaderId",
                        column: x => x.CurrentReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Books_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Country", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"), "Russia", new DateOnly(1821, 11, 11), "Fedor", "Dostoyevskiy" },
                    { new Guid("21cb29da-047a-4d85-a581-8ef6cffec67f"), "Russia", new DateOnly(1868, 3, 28), "Maxim", "Gorkiy" },
                    { new Guid("3bc8f089-2d00-4346-af71-d9f9fcdceb20"), "Japan", new DateOnly(1995, 7, 3), "Miya", "Kazuki" },
                    { new Guid("4792ce31-a3e8-4df3-b0d7-4ea1c8e40dbd"), "United Kingdom", new DateOnly(1564, 4, 23), "William", "Shakespeare" },
                    { new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"), "Russia", new DateOnly(1828, 9, 9), "Lev", "Tolstoy" },
                    { new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"), "Belarus", new DateOnly(1924, 6, 22), "Vasil", "Bykov" },
                    { new Guid("ec891ac2-f620-415f-9f86-3d15259eb071"), "Russia", new DateOnly(1809, 1, 4), "Nikolay", "Gogol" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "AuthorName", "BorrowTime", "CurrentReaderId", "Description", "Genre", "ISBN", "ImagePath", "IsAvailable", "ReaderId", "ReturnTime", "Title" },
                values: new object[,]
                {
                    { new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"), new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"), "Lev Tolstoy", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Novel", "ISBN 13: 9781566190275", null, true, null, null, "War and Peace" },
                    { new Guid("2f346383-bd6a-4564-8dce-343c355e795a"), new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"), "Lev Tolstoy", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Novel", "ISBN 13: 9780672523830", null, true, null, null, "Anna Karenina" },
                    { new Guid("424e64e8-c811-42ef-8153-f7952ced8c51"), new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"), "Fedor Dostoyevskiy", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Novel", "ISBN 10: 0374528373", null, true, null, null, "The Brothers Karamazov" },
                    { new Guid("6328fcf9-5846-4f7c-960c-da5ea5c32f22"), new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"), "Fedor Dostoyevskiy", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Novel", "ISBN 13: 9785050000149", null, true, null, null, "Crime and Punishment" },
                    { new Guid("81ebde25-7b81-4bf2-8691-edef624642d8"), new Guid("ec891ac2-f620-415f-9f86-3d15259eb071"), "Nikolay Gogol", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Satire", "ISBN 10: 0300060998", null, true, null, null, "Dead souls" },
                    { new Guid("8e32b21e-1a32-4272-bc46-6f7b709a7696"), new Guid("4792ce31-a3e8-4df3-b0d7-4ea1c8e40dbd"), "William Shakespeare", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Shakespearean tragedy", "ISBN 13: 9780671722852", null, true, null, null, "Romeo and Juliet" },
                    { new Guid("a0283873-60b8-45de-a411-02a0a3fbc465"), new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"), "Vasil Bykov", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "War novel", "ISBN 13: 9781909156821", null, true, null, null, "Alpine Ballad" },
                    { new Guid("ad9c4dbe-5dff-43e0-a58c-cea9327a4464"), new Guid("21cb29da-047a-4d85-a581-8ef6cffec67f"), "Maxim Gorkiy", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Friction", "ISBN-13: 9798390533352", null, true, null, null, "Old Izergil" },
                    { new Guid("f31001c4-fb5d-42f0-aafd-dd0e6e08476e"), new Guid("3bc8f089-2d00-4346-af71-d9f9fcdceb20"), "Miya Kazuki", null, null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc varius rhoncus nisl, nec egestas lacus pellentesque vitae. Donec eleifend urna at nunc tincidunt facilisis. Nam consectetur odio erat sed.", "Friction", "ISBN-13: 9781718357976", null, true, null, null, "Ascendance of a bookworm" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CurrentReaderId",
                table: "Books",
                column: "CurrentReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReaderId",
                table: "Books",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_Email",
                table: "Readers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Readers");
        }
    }
}
