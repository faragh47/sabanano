using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderManagement3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAnalyze_AnalyzerDevice_AnalyzerDeviceId",
                table: "OrderAnalyze");

            migrationBuilder.DropIndex(
                name: "IX_OrderAnalyze_AnalyzerDeviceId",
                table: "OrderAnalyze");

            migrationBuilder.DropColumn(
                name: "AnalyzerDeviceId",
                table: "OrderAnalyze");

            migrationBuilder.AlterColumn<int>(
                name: "AnalyzeDeviceId",
                table: "OrderAnalyze",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAnalyze_AnalyzeDeviceId",
                table: "OrderAnalyze",
                column: "AnalyzeDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAnalyze_AnalyzerDevice_AnalyzeDeviceId",
                table: "OrderAnalyze",
                column: "AnalyzeDeviceId",
                principalTable: "AnalyzerDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAnalyze_AnalyzerDevice_AnalyzeDeviceId",
                table: "OrderAnalyze");

            migrationBuilder.DropIndex(
                name: "IX_OrderAnalyze_AnalyzeDeviceId",
                table: "OrderAnalyze");

            migrationBuilder.AlterColumn<long>(
                name: "AnalyzeDeviceId",
                table: "OrderAnalyze",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AnalyzerDeviceId",
                table: "OrderAnalyze",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderAnalyze_AnalyzerDeviceId",
                table: "OrderAnalyze",
                column: "AnalyzerDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAnalyze_AnalyzerDevice_AnalyzerDeviceId",
                table: "OrderAnalyze",
                column: "AnalyzerDeviceId",
                principalTable: "AnalyzerDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
