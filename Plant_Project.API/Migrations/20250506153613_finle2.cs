using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class finle2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_plants_PlantId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "PlantId",
                table: "Carts",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_PlantId",
                table: "Carts",
                newName: "IX_Carts_ItemId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKmjcktOkcQ0xUqLDwC1skeI69OEQyOgbYDXkNiKh8bqcmO5Te0sJJOwE4VD2WczxA==");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_plants_ItemId",
                table: "Carts",
                column: "ItemId",
                principalTable: "plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_plants_ItemId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Carts",
                newName: "PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ItemId",
                table: "Carts",
                newName: "IX_Carts_PlantId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_plants_PlantId",
                table: "Carts",
                column: "PlantId",
                principalTable: "plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
