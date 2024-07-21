using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(498), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(499) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("21cb29da-047a-4d85-a581-8ef6cffec67f"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(459), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(460) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("3bc8f089-2d00-4346-af71-d9f9fcdceb20"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(494), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(495) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4792ce31-a3e8-4df3-b0d7-4ea1c8e40dbd"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(509), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(510) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(444), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(455) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(512), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(512) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("ec891ac2-f620-415f-9f86-3d15259eb071"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(501), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(502) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(732), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(734) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("2f346383-bd6a-4564-8dce-343c355e795a"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(738), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(739) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("424e64e8-c811-42ef-8153-f7952ced8c51"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(751), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(752) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6328fcf9-5846-4f7c-960c-da5ea5c32f22"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(748), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(749) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("81ebde25-7b81-4bf2-8691-edef624642d8"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(754), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(755) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("8e32b21e-1a32-4272-bc46-6f7b709a7696"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(758), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(759) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("a0283873-60b8-45de-a411-02a0a3fbc465"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(762), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(762) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("ad9c4dbe-5dff-43e0-a58c-cea9327a4464"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(742), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(742) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f31001c4-fb5d-42f0-aafd-dd0e6e08476e"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(745), new DateTime(2024, 7, 21, 10, 56, 15, 879, DateTimeKind.Local).AddTicks(745) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("188ec0f1-b4a1-4a86-9bb4-f249c2a1032b"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("21cb29da-047a-4d85-a581-8ef6cffec67f"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("3bc8f089-2d00-4346-af71-d9f9fcdceb20"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4792ce31-a3e8-4df3-b0d7-4ea1c8e40dbd"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("ec891ac2-f620-415f-9f86-3d15259eb071"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("2f346383-bd6a-4564-8dce-343c355e795a"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("424e64e8-c811-42ef-8153-f7952ced8c51"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6328fcf9-5846-4f7c-960c-da5ea5c32f22"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("81ebde25-7b81-4bf2-8691-edef624642d8"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("8e32b21e-1a32-4272-bc46-6f7b709a7696"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("a0283873-60b8-45de-a411-02a0a3fbc465"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("ad9c4dbe-5dff-43e0-a58c-cea9327a4464"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f31001c4-fb5d-42f0-aafd-dd0e6e08476e"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
