using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class notifiaction3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJEHMfEIyCTjCVVfUBQplaq7pzPPNTFs1rVnIZ4DQ+TOtTVpGTfrJkF9pHGobWEb7Q==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEChU8rdEGgxbR10VVW2eUElO5em44u5o/2AHtn1n+uKOS2oYJypFYxlBCx0cKHwRPw==");
        }
    }
}
