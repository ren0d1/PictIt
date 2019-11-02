using Microsoft.EntityFrameworkCore.Migrations;

namespace PictIt.Migrations
{
    public partial class FileExtensionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Pictures");
        }
    }
}
