using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInAnalyzeDevices4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalyzerDeviceSample_MaintenanceCondition_MaintenanceConditionId",
                table: "AnalyzerDeviceSample");

            migrationBuilder.DropIndex(
                name: "IX_AnalyzerDeviceSample_MaintenanceConditionId",
                table: "AnalyzerDeviceSample");

            migrationBuilder.DropColumn(
                name: "MaintenanceConditionId",
                table: "AnalyzerDeviceSample");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaintenanceConditionId",
                table: "AnalyzerDeviceSample",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzerDeviceSample_MaintenanceConditionId",
                table: "AnalyzerDeviceSample",
                column: "MaintenanceConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalyzerDeviceSample_MaintenanceCondition_MaintenanceConditionId",
                table: "AnalyzerDeviceSample",
                column: "MaintenanceConditionId",
                principalTable: "MaintenanceCondition",
                principalColumn: "Id");
        }
    }
}
