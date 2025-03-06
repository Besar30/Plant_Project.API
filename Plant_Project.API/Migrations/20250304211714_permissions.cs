using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Plant_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89", 0, "bad19ffe-f206-4276-a443-b89ca8eb8349", "admin@Plant-Project.com", false, "Plant-Project", "Admin", false, null, "ADMIN@PLANT-PROJECT.COM", "ADMIN@PLANT-PROJECT.COM", "AQAAAAIAAYagAAAAEK07pFwMqGCDCwExBp1nTdBa3XFfaaYIS+AB1t9tRheDHO78nWpUP2EV6wEczfPJoQ==", null, false, "47806FAABE5C436785E0DA748763DF68", false, "Plant-Project" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "Plant:read", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 2, "Permissions", "Plant:Add", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 3, "Permissions", "Plant:Delete", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 4, "Permissions", "Plant:Update", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 5, "Permissions", "Category:read", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 6, "Permissions", "Category:Add", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 7, "Permissions", "Category:Delete", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 8, "Permissions", "Category:Update", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 9, "Permissions", "Users:read", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 10, "Permissions", "Users:Add", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 11, "Permissions", "Users:Update", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 12, "Permissions", "Roles:read", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 13, "Permissions", "Roles:Add", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 14, "Permissions", "Roles:Update", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" },
                    { 15, "Permissions", "results:read", "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b0b0ca4f-21d5-4d35-942e-74c5f2e1ee8c", "6bd9d8f6-9497-4fd5-9f50-7d5dd6ab5c89" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

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
        }
    }
}
