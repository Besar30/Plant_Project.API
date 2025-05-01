using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class updatereact1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LikedAt",
                table: "React",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB4gUXQ3B4rVTSruXOweO4E4yXzg7xJh7oYvauPDMJ9Epd+L5G/NSGTcPdHHvBg21g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedAt",
                table: "React");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB6XBM9fVx/954iyDtW5R4sL1PmD9UJzz6nY2WM2IwJdjdYFVzqfScBh4YcSx2qEeA==");
        }
    }
}
