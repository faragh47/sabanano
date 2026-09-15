using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInAnalyzeDevices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ImageId",
                table: "AnalyzeDeviceResponse",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "AnalyzeDeviceId",
                table: "AnalyzeDeviceResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzeDeviceResponse_AnalyzeDeviceId",
                table: "AnalyzeDeviceResponse",
                column: "AnalyzeDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalyzeDeviceResponse_AnalyzerDevice_AnalyzeDeviceId",
                table: "AnalyzeDeviceResponse",
                column: "AnalyzeDeviceId",
                principalTable: "AnalyzerDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalyzeDeviceResponse_AnalyzerDevice_AnalyzeDeviceId",
                table: "AnalyzeDeviceResponse");

            migrationBuilder.DropIndex(
                name: "IX_AnalyzeDeviceResponse_AnalyzeDeviceId",
                table: "AnalyzeDeviceResponse");

            migrationBuilder.DropColumn(
                name: "AnalyzeDeviceId",
                table: "AnalyzeDeviceResponse");

            migrationBuilder.AlterColumn<long>(
                name: "ImageId",
                table: "AnalyzeDeviceResponse",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
