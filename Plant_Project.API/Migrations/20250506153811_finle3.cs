using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class finle3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJW+KRYf7RrsQdZN8xVBuFLgk5cbkKKcHp+smgq+d432McHv4yeFti9P9+KGnkbhWw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKmjcktOkcQ0xUqLDwC1skeI69OEQyOgbYDXkNiKh8bqcmO5Te0sJJOwE4VD2WczxA==");
        }
    }
}
