using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class addimageinTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                columns: new[] { "ImagePath", "PasswordHash" },
                values: new object[] { "", "AQAAAAIAAYagAAAAEHi6kbWDa4oCZSV7eY5yxxVYKjqifkO5yT1DdbNRagYiJ1auwbC0SPV3RU/+l8/egw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK07pFwMqGCDCwExBp1nTdBa3XFfaaYIS+AB1t9tRheDHO78nWpUP2EV6wEczfPJoQ==");
        }
    }
}
