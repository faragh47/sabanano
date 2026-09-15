using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPart2Analyzes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SEM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zoom = table.Column<double>(type: "float", nullable: true),
                    Size = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TEM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zoom = table.Column<double>(type: "float", nullable: true),
                    Size = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TGA",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TempertureStart = table.Column<double>(type: "float", nullable: true),
                    TempertureEnd = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Environment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TGA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UV",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaveLengthStart = table.Column<double>(type: "float", nullable: true),
                    WaveLengthEnd = table.Column<double>(type: "float", nullable: true),
                    Spectrum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UV", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XRD",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AngleStart = table.Column<double>(type: "float", nullable: false),
                    AngleEnd = table.Column<double>(type: "float", nullable: false),
                    Composition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNeedGrind = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XRD", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEM");

            migrationBuilder.DropTable(
                name: "TEM");

            migrationBuilder.DropTable(
                name: "TGA");

            migrationBuilder.DropTable(
                name: "UV");

            migrationBuilder.DropTable(
                name: "XRD");
        }
    }
}
