using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a3ff3c8-534e-4822-a2f0-86e6869abaee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9d67aa2-20d8-424f-a3dc-4e427ee5a3ae");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e9fdf28-e712-48cf-8456-68f3bfd8d5bf", "1", "Admin", "Admin" },
                    { "f4839e79-d5d1-41b6-8410-9412d7ffedac", "2", "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserName",
                table: "Products",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserName",
                table: "Products",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserName",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e9fdf28-e712-48cf-8456-68f3bfd8d5bf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4839e79-d5d1-41b6-8410-9412d7ffedac");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a3ff3c8-534e-4822-a2f0-86e6869abaee", "2", "User", "User" },
                    { "f9d67aa2-20d8-424f-a3dc-4e427ee5a3ae", "1", "Admin", "Admin" }
                });
        }
    }
}
