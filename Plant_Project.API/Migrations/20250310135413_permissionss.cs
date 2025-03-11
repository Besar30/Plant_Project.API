using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class permissionss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBta1DBZg1VAVYnL77lz3AAeFUpqwVonJ1zA553E1WDebpZvxmIqmJHyXJZLSqjqDA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHi6kbWDa4oCZSV7eY5yxxVYKjqifkO5yT1DdbNRagYiJ1auwbC0SPV3RU/+l8/egw==");
        }
    }
}
