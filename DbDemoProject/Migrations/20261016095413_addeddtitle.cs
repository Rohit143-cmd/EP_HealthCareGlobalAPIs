using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbDemoProject.Migrations
{
    /// <inheritdoc />
    public partial class addeddtitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "DashboardItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "DashboardItems");
        }
    }
}
