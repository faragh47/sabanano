using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderManagement2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAnalyze_Safety_SafetyId",
                table: "OrderAnalyze");

            migrationBuilder.AlterColumn<long>(
                name: "SafetyId",
                table: "OrderAnalyze",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAnalyze_Safety_SafetyId",
                table: "OrderAnalyze",
                column: "SafetyId",
                principalTable: "Safety",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAnalyze_Safety_SafetyId",
                table: "OrderAnalyze");

            migrationBuilder.AlterColumn<long>(
                name: "SafetyId",
                table: "OrderAnalyze",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAnalyze_Safety_SafetyId",
                table: "OrderAnalyze",
                column: "SafetyId",
                principalTable: "Safety",
                principalColumn: "Id");
        }
    }
}
