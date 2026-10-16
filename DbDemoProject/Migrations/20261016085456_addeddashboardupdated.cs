using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbDemoProject.Migrations
{
    /// <inheritdoc />
    public partial class addeddashboardupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DashboardItems",
                columns: table => new
                {
                    DashbordUUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleUUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<int>(type: "int", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gradient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shadowColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardItems", x => x.DashbordUUID);
                    table.ForeignKey(
                        name: "FK_DashboardItems_Roles_RoleUUID",
                        column: x => x.RoleUUID,
                        principalTable: "Roles",
                        principalColumn: "RoleUUID");
                });
           
            migrationBuilder.CreateIndex(
                name: "IX_DashboardItems_RoleUUID",
                table: "DashboardItems",
                column: "RoleUUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardItems");
        }
    }
}
