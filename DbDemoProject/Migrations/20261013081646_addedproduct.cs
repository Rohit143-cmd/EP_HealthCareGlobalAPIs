using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbDemoProject.Migrations
{
    /// <inheritdoc />
    public partial class addedproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductUUID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductUUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductUUID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProductUUID",
                table: "Users",
                column: "ProductUUID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Product_ProductUUID",
                table: "Users",
                column: "ProductUUID",
                principalTable: "Product",
                principalColumn: "ProductUUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Product_ProductUUID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProductUUID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductUUID",
                table: "Users");
        }
    }
}
