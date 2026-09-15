using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changesurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "OrderSurvey");

            migrationBuilder.AddColumn<int>(
                name: "ScoreId",
                table: "OrderSurvey",
                type: "int",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "ScoreTitle",
                table: "OrderSurvey",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "متوسط");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "OrderSurvey");

            migrationBuilder.DropColumn(
                name: "ScoreTitle",
                table: "OrderSurvey");

            migrationBuilder.AddColumn<string>(
                name: "Score",
                table: "OrderSurvey",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
