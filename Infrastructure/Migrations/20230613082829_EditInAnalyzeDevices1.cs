using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInAnalyzeDevices1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AnalyzerDevice",
                newName: "PersianName");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AnalyzerDevice",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AnalyzerDevice");

            migrationBuilder.RenameColumn(
                name: "PersianName",
                table: "AnalyzerDevice",
                newName: "Title");
        }
    }
}
