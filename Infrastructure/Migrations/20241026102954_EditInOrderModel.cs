using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequireHeader",
                table: "OrderAnalyze");

            migrationBuilder.DropColumn(
                name: "IsRequireToReturnSample",
                table: "OrderAnalyze");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequireHeader",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequireToReturnSample",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequireHeader",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsRequireToReturnSample",
                table: "Order");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequireHeader",
                table: "OrderAnalyze",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequireToReturnSample",
                table: "OrderAnalyze",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
