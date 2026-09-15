using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPersonInOrder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PersonId",
                table: "Order",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Person_PersonId",
                table: "Order",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Person_PersonId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PersonId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Order");
        }
    }
}
