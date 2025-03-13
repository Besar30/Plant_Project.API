using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class addimagepathtocategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECcpJIr5+IG9BcIs1RCnirm6T9JFABN+/aHGzr5Xy7KZ4pirLN7Vgc+Xx2+3QNZ4UQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "categories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBqoc3y6FremIkGdXRlCLmjuQrrLwXu0piFPwBYiid/rnhJyvlCd1nzbLExKTDHF6Q==");
        }
    }
}
