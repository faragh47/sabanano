using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageResultInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ImageId",
                table: "Order",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Image_ImageId",
                table: "Order",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Image_ImageId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ImageId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Order");
        }
    }
}
