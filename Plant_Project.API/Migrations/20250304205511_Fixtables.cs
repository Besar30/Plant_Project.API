using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class Fixtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP7gpQVJPrHMySFS3dhV2G/r/b66cQQj6M0at8ZGtfpRnzGeYNRCuJSPTQWma7l/cA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHjqxfd8Z/ldzJOvQvaZwXhNnkBTGM2+MVQyzT4l6rjSNaDy03puQfwZm5S661aN3Q==");
        }
    }
}
