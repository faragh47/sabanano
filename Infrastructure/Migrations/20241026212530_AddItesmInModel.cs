using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItesmInModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasNoSafety",
                table: "Safety",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExplosive",
                table: "Safety",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasNotAnyCondition",
                table: "OrderAnalyze",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Grant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Grant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TelNumber",
                table: "Grant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniversityName",
                table: "Grant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "واریز مستقیم");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasNoSafety",
                table: "Safety");

            migrationBuilder.DropColumn(
                name: "IsExplosive",
                table: "Safety");

            migrationBuilder.DropColumn(
                name: "HasNotAnyCondition",
                table: "OrderAnalyze");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Grant");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Grant");

            migrationBuilder.DropColumn(
                name: "TelNumber",
                table: "Grant");

            migrationBuilder.DropColumn(
                name: "UniversityName",
                table: "Grant");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "واریز مستقیم به ایران فود");
        }
    }
}
