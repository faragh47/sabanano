using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "AnalyzerDevice",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "AnalyzerDevice",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Safety",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPoisonous = table.Column<bool>(type: "bit", nullable: false),
                    IsEscapable = table.Column<bool>(type: "bit", nullable: false),
                    IsFlammable = table.Column<bool>(type: "bit", nullable: false),
                    IsBadForBreathing = table.Column<bool>(type: "bit", nullable: false),
                    IsAdsorbBySkin = table.Column<bool>(type: "bit", nullable: false),
                    IsNanoSize = table.Column<bool>(type: "bit", nullable: false),
                    IsSickness = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Safety", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderAnalyze",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequireToReturnSample = table.Column<bool>(type: "bit", nullable: false),
                    IsRequireHeader = table.Column<bool>(type: "bit", nullable: false),
                    IsSensitiveToLight = table.Column<bool>(type: "bit", nullable: false),
                    IsSensitiveToHumidity = table.Column<bool>(type: "bit", nullable: false),
                    SpeceficTemperture = table.Column<double>(type: "float", nullable: true),
                    SpeceficAtmosphere = table.Column<double>(type: "float", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    SafetyId = table.Column<long>(type: "bigint", nullable: true),
                    AnalyzeDeviceId = table.Column<long>(type: "bigint", nullable: false),
                    AnalyzerDeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAnalyze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAnalyze_AnalyzerDevice_AnalyzerDeviceId",
                        column: x => x.AnalyzerDeviceId,
                        principalTable: "AnalyzerDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderAnalyze_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderAnalyze_Safety_SafetyId",
                        column: x => x.SafetyId,
                        principalTable: "Safety",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BET",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegassingTemperature = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<double>(type: "float", nullable: false),
                    OrderAnalyzeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BET_OrderAnalyze_OrderAnalyzeId",
                        column: x => x.OrderAnalyzeId,
                        principalTable: "OrderAnalyze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzerDevice_OrderId",
                table: "AnalyzerDevice",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BET_OrderAnalyzeId",
                table: "BET",
                column: "OrderAnalyzeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAnalyze_AnalyzerDeviceId",
                table: "OrderAnalyze",
                column: "AnalyzerDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAnalyze_OrderId",
                table: "OrderAnalyze",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAnalyze_SafetyId",
                table: "OrderAnalyze",
                column: "SafetyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalyzerDevice_Order_OrderId",
                table: "AnalyzerDevice",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalyzerDevice_Order_OrderId",
                table: "AnalyzerDevice");

            migrationBuilder.DropTable(
                name: "BET");

            migrationBuilder.DropTable(
                name: "OrderAnalyze");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Safety");

            migrationBuilder.DropIndex(
                name: "IX_AnalyzerDevice_OrderId",
                table: "AnalyzerDevice");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "AnalyzerDevice");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AnalyzerDevice");
        }
    }
}
