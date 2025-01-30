using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Score", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c6c6c6c6-0000-4000-a000-000000000003", 0, "11223344-5566-7788-99AA-BBCCDDEEFF00", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJ6fTxzqMkdqD8i8TTj7EvUGQFUKgFgEej7VJbXZUK4rPZ0dfmjR2O+GPlM+Ep6A==", null, false, null, "A1B2C3D4E5F60708A9B0C1D2E3F4G5H6", false, "admin@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6c6c6c6-0000-4000-a000-000000000003");
        }
    }
}
