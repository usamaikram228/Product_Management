using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserName",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e9fdf28-e712-48cf-8456-68f3bfd8d5bf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4839e79-d5d1-41b6-8410-9412d7ffedac");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Products",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UserName",
                table: "Products",
                newName: "IX_Products_UserId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34616997-cb81-4129-97ed-caf7c39f1a78", "1", "Admin", "Admin" },
                    { "7c05efa2-7801-4b8e-bf1c-b34e08c400b3", "2", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserId",
                table: "Products",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34616997-cb81-4129-97ed-caf7c39f1a78");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c05efa2-7801-4b8e-bf1c-b34e08c400b3");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Products",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UserId",
                table: "Products",
                newName: "IX_Products_UserName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e9fdf28-e712-48cf-8456-68f3bfd8d5bf", "1", "Admin", "Admin" },
                    { "f4839e79-d5d1-41b6-8410-9412d7ffedac", "2", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserName",
                table: "Products",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
