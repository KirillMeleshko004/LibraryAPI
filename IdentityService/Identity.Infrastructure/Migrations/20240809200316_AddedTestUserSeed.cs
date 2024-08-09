using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTestUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a0649f7d-dc4b-4a5f-8cc3-602c41c1a2dd"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f780ab99-f4d8-403a-8a17-f6f5f2946699"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("ca0b4257-5277-46e3-be6a-267319420ac4"), null, "Admin", "ADMIN" },
                    { new Guid("cca6b1ef-9373-4b47-9d7e-56c3ac31fb0d"), null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e"), 0, "9f58ae9c-36ca-4c33-a327-f017cf33dc9a", "testuser@gmail.com", true, "Kirill", "Meleshko", false, null, null, "KIRILLL", "AQAAAAIAAYagAAAAEA5caD2EhYehXubogT/nV+kS+8DPunSIdk15c7iRJTuWx0ZbTxHZ+96/Q1KoOhPNmQ==", null, false, null, false, "kirilll" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("ca0b4257-5277-46e3-be6a-267319420ac4"), new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e") },
                    { new Guid("cca6b1ef-9373-4b47-9d7e-56c3ac31fb0d"), new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("ca0b4257-5277-46e3-be6a-267319420ac4"), new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cca6b1ef-9373-4b47-9d7e-56c3ac31fb0d"), new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ca0b4257-5277-46e3-be6a-267319420ac4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cca6b1ef-9373-4b47-9d7e-56c3ac31fb0d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88081a15-3dae-49c8-96e4-d0f554dd9e0e"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a0649f7d-dc4b-4a5f-8cc3-602c41c1a2dd"), null, "Customer", "CUSTOMER" },
                    { new Guid("f780ab99-f4d8-403a-8a17-f6f5f2946699"), null, "Admin", "ADMIN" }
                });
        }
    }
}
