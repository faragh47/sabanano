using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderManagement1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalyzerDevice_Order_OrderId",
                table: "AnalyzerDevice");

            migrationBuilder.DropIndex(
                name: "IX_AnalyzerDevice_OrderId",
                table: "AnalyzerDevice");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AnalyzerDevice");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OrderAnalyze",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalDescription",
                table: "OrderAnalyze",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OrderAnalyze",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalDescription",
                table: "OrderAnalyze",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "AnalyzerDevice",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzerDevice_OrderId",
                table: "AnalyzerDevice",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalyzerDevice_Order_OrderId",
                table: "AnalyzerDevice",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
