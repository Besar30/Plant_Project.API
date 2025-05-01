using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class dbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                table: "React");

            migrationBuilder.DropForeignKey(
                name: "FK_React_Posts_PostId",
                table: "React");

            migrationBuilder.DropPrimaryKey(
                name: "PK_React",
                table: "React");

            migrationBuilder.RenameTable(
                name: "React",
                newName: "Reacts");

            migrationBuilder.RenameIndex(
                name: "IX_React_UserId",
                table: "Reacts",
                newName: "IX_Reacts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_React_PostId",
                table: "Reacts",
                newName: "IX_Reacts_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reacts",
                table: "Reacts",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBt32XAexaF6qfZxN0ktsBCmjIvmsBWMW5dXgwVsJbM1GUxgKMuZKXnSFJVeadHxRA==");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_AspNetUsers_UserId",
                table: "Reacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Posts_PostId",
                table: "Reacts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_AspNetUsers_UserId",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Posts_PostId",
                table: "Reacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reacts",
                table: "Reacts");

            migrationBuilder.RenameTable(
                name: "Reacts",
                newName: "React");

            migrationBuilder.RenameIndex(
                name: "IX_Reacts_UserId",
                table: "React",
                newName: "IX_React_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reacts_PostId",
                table: "React",
                newName: "IX_React_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_React",
                table: "React",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB4gUXQ3B4rVTSruXOweO4E4yXzg7xJh7oYvauPDMJ9Epd+L5G/NSGTcPdHHvBg21g==");

            migrationBuilder.AddForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                table: "React",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_React_Posts_PostId",
                table: "React",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
