using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFinancialInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FinancialId",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_FinancialId",
                table: "Order",
                column: "FinancialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Financial_FinancialId",
                table: "Order",
                column: "FinancialId",
                principalTable: "Financial",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Financial_FinancialId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_FinancialId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FinancialId",
                table: "Order");
        }
    }
}
