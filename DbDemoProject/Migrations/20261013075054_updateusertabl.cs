using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbDemoProject.Migrations
{
    /// <inheritdoc />
    public partial class updateusertabl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleUUID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleUUID1",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleUUID1",
                table: "Users",
                column: "RoleUUID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleUUID1",
                table: "Users",
                column: "RoleUUID1",
                principalTable: "Roles",
                principalColumn: "RoleUUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleUUID1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleUUID1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleUUID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleUUID1",
                table: "Users");
        }
    }
}
