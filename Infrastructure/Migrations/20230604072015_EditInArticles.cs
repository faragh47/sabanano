using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditInArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderName",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Article",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Article_ImageId",
                table: "Article",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Image_ImageId",
                table: "Article",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Image_ImageId",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_ImageId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "HeaderName",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Article");
        }
    }
}
