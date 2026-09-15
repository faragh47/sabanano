using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInFinacial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Financial",
                newName: "TotalPayablePrice");

            migrationBuilder.AddColumn<decimal>(
                name: "AnalyzePrice",
                table: "Financial",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrantPrice",
                table: "Financial",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Financial",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalyzePrice",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "GrantPrice",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Financial");

            migrationBuilder.RenameColumn(
                name: "TotalPayablePrice",
                table: "Financial",
                newName: "TotalPrice");
        }
    }
}
