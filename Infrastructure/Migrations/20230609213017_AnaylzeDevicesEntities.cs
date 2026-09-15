using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AnaylzeDevicesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalyzeDeviceResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzeDeviceResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzeDeviceResponse_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzerDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpectroscopyRange = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaintenanceCondition = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,10)", precision: 10, scale: 10, nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    DescriptionForReadyAnalyze = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzerDevice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceCondition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SampleCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzeDeviceAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalyzeDeviceId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzeDeviceAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzeDeviceAttribute_AnalyzerDevice_AnalyzeDeviceId",
                        column: x => x.AnalyzeDeviceId,
                        principalTable: "AnalyzerDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzeDeviceInput",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalyzeDeviceId = table.Column<int>(type: "int", nullable: false),
                    isList = table.Column<bool>(type: "bit", nullable: true),
                    TextBox = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCheckbox = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzeDeviceInput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzeDeviceInput_AnalyzerDevice_AnalyzeDeviceId",
                        column: x => x.AnalyzeDeviceId,
                        principalTable: "AnalyzerDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzeDeviceService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalyzeDeviceId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,10)", precision: 10, scale: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzeDeviceService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzeDeviceService_AnalyzerDevice_AnalyzeDeviceId",
                        column: x => x.AnalyzeDeviceId,
                        principalTable: "AnalyzerDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzerDeviceSample",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleCategoryId = table.Column<int>(type: "int", nullable: false),
                    AnalyerDeviceId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceConditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzerDeviceSample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzerDeviceSample_AnalyzerDevice_AnalyerDeviceId",
                        column: x => x.AnalyerDeviceId,
                        principalTable: "AnalyzerDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalyzerDeviceSample_MaintenanceCondition_MaintenanceConditionId",
                        column: x => x.MaintenanceConditionId,
                        principalTable: "MaintenanceCondition",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnalyzerDeviceSample_SampleCategory_SampleCategoryId",
                        column: x => x.SampleCategoryId,
                        principalTable: "SampleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzeDeviceAttributeDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalyzeAttributeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzeDeviceAttributeDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzeDeviceAttributeDetail_AnalyzeDeviceAttribute_AnalyzeAttributeId",
                        column: x => x.AnalyzeAttributeId,
                        principalTable: "AnalyzeDeviceAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzeDeviceAttribute_AnalyzeDeviceId",
                table: "AnalyzeDeviceAttribute",
                column: "AnalyzeDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzeDeviceAttributeDetail_AnalyzeAttributeId",
                table: "AnalyzeDeviceAttributeDetail",
                column: "AnalyzeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzeDeviceInput_AnalyzeDeviceId",
                table: "AnalyzeDeviceInput",
                column: "AnalyzeDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzeDeviceResponse_ImageId",
                table: "AnalyzeDeviceResponse",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzeDeviceService_AnalyzeDeviceId",
                table: "AnalyzeDeviceService",
                column: "AnalyzeDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzerDeviceSample_AnalyerDeviceId",
                table: "AnalyzerDeviceSample",
                column: "AnalyerDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzerDeviceSample_MaintenanceConditionId",
                table: "AnalyzerDeviceSample",
                column: "MaintenanceConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzerDeviceSample_SampleCategoryId",
                table: "AnalyzerDeviceSample",
                column: "SampleCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyzeDeviceAttributeDetail");

            migrationBuilder.DropTable(
                name: "AnalyzeDeviceInput");

            migrationBuilder.DropTable(
                name: "AnalyzeDeviceResponse");

            migrationBuilder.DropTable(
                name: "AnalyzeDeviceService");

            migrationBuilder.DropTable(
                name: "AnalyzerDeviceSample");

            migrationBuilder.DropTable(
                name: "AnalyzeDeviceAttribute");

            migrationBuilder.DropTable(
                name: "MaintenanceCondition");

            migrationBuilder.DropTable(
                name: "SampleCategory");

            migrationBuilder.DropTable(
                name: "AnalyzerDevice");
        }
    }
}
