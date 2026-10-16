using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbDemoProject.Migrations
{
    /// <inheritdoc />
    public partial class addedbuttonlable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "buttonLabel",
                table: "DashboardItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "buttonLabel",
                table: "DashboardItems");
        }
    }
}
