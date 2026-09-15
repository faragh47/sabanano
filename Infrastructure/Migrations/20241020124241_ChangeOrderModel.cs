using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BET_OrderAnalyze_OrderAnalyzeId",
                table: "BET");

            migrationBuilder.DropIndex(
                name: "IX_BET_OrderAnalyzeId",
                table: "BET");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BET");

            migrationBuilder.DropColumn(
                name: "OrderAnalyzeId",
                table: "BET");

            migrationBuilder.AddColumn<long>(
                name: "AnalyzeModelId",
                table: "OrderAnalyze",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderAnalyze",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalyzeModelId",
                table: "OrderAnalyze");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderAnalyze");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BET",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "OrderAnalyzeId",
                table: "BET",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_BET_OrderAnalyzeId",
                table: "BET",
                column: "OrderAnalyzeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BET_OrderAnalyze_OrderAnalyzeId",
                table: "BET",
                column: "OrderAnalyzeId",
                principalTable: "OrderAnalyze",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
