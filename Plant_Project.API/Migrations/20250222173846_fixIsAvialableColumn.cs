using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class fixIsAvialableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_Avilable",
                table: "Plants",
                newName: "Is_Available");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAz/9Rd2EIdV5dqgz0CzhnOGS9i1gapg9Y0NOg3Lnr3aDim6T0vuNF/9MLuXUMN4Xw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_Available",
                table: "Plants",
                newName: "Is_Avilable");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFJUX+yXqLuFR1R1Nvna89uDMNb3xLOWSu/AKs195FtJ51vyESpB3cAanNUJ2JhP2A==");
        }
    }
}
