using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class final1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOfgmU0PbM0LRCtrccTcEsIydnpHWRYboy70jp0s8Ly31ukHnn860BU0kX62FZwudQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Carts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBt32XAexaF6qfZxN0ktsBCmjIvmsBWMW5dXgwVsJbM1GUxgKMuZKXnSFJVeadHxRA==");
        }
    }
}
