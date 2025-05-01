using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class React : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75b5f503-cd63-4bc6-bce9-225d1d8b9371");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c", "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89");

            migrationBuilder.CreateTable(
                name: "React",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_React", x => x.Id);
                    table.ForeignKey(
                        name: "FK_React_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_React_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15,
                column: "RoleId",
                value: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0195442f-5b32-7334-9a35-d43ff70d3aa9", "0195442f-5b32-761a-b2ee-cfca69434828", false, false, "Admin", "ADMIN" },
                    { "0195442f-5b32-7b00-a097-61b7c3baec76", "0195442f-5b32-7bfc-8b9c-18f34c1d2eea", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImagePath", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0195442f-5b32-7163-9117-b7023daacb2d", 0, "0195442f-5b32-7594-8754-260776e9cdcc", "admin@Plant-Project.com", false, "Plant-Project", "", "Admin", false, null, "ADMIN@PLANT-PROJECT.COM", "ADMIN@PLANT-PROJECT.COM", "AQAAAAIAAYagAAAAEB6XBM9fVx/954iyDtW5R4sL1PmD9UJzz6nY2WM2IwJdjdYFVzqfScBh4YcSx2qEeA==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "Plant-Project" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0195442f-5b32-7334-9a35-d43ff70d3aa9", "0195442f-5b32-7163-9117-b7023daacb2d" });

            migrationBuilder.CreateIndex(
                name: "IX_React_PostId",
                table: "React",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_React_UserId",
                table: "React",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "React");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7b00-a097-61b7c3baec76");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0195442f-5b32-7334-9a35-d43ff70d3aa9", "0195442f-5b32-7163-9117-b7023daacb2d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7334-9a35-d43ff70d3aa9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195442f-5b32-7163-9117-b7023daacb2d");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15,
                column: "RoleId",
                value: "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "75b5f503-cd63-4bc6-bce9-225d1d8b9371", "b4fe2a13-c767-4f0b-8ce2-18a908b0df1f", true, false, "Member", "MEMBER" },
                    { "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c", "12ec1a44-a350-4626-af23-28882dfd3c5e", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImagePath", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89", 0, "bad19ffe-f206-4276-a443-b89ca8eb8349", "admin@Plant-Project.com", false, "Plant-Project", "", "Admin", false, null, "ADMIN@PLANT-PROJECT.COM", "ADMIN@PLANT-PROJECT.COM", "AQAAAAIAAYagAAAAEJEHMfEIyCTjCVVfUBQplaq7pzPPNTFs1rVnIZ4DQ+TOtTVpGTfrJkF9pHGobWEb7Q==", null, false, "47806FAABE5C436785E0DA748763DF68", false, "Plant-Project" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c", "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89" });
        }
    }
}
