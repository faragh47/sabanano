using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGrantAndCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GrantId",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegisterCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EconomicNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grant", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CompanyId",
                table: "Order",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_GrantId",
                table: "Order",
                column: "GrantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Company_CompanyId",
                table: "Order",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Grant_GrantId",
                table: "Order",
                column: "GrantId",
                principalTable: "Grant",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Company_CompanyId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Grant_GrantId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Grant");

            migrationBuilder.DropIndex(
                name: "IX_Order_CompanyId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_GrantId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "GrantId",
                table: "Order");
        }
    }
}
