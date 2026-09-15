using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditAnalyzerDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalyzeDeviceResponse_Image_ImageId",
                table: "AnalyzeDeviceResponse");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionForReadyAnalyze",
                table: "AnalyzerDevice",
                type: "nvarchar(1200)",
                maxLength: 1200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalyzeDeviceResponse_Image_ImageId",
                table: "AnalyzeDeviceResponse",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalyzeDeviceResponse_Image_ImageId",
                table: "AnalyzeDeviceResponse");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionForReadyAnalyze",
                table: "AnalyzerDevice",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1200)",
                oldMaxLength: 1200);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalyzeDeviceResponse_Image_ImageId",
                table: "AnalyzeDeviceResponse",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
