using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "difficulties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_difficulties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roads",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LengthInKm = table.Column<double>(type: "float", nullable: false),
                    RoadImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    difficultyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roads", x => x.id);
                    table.ForeignKey(
                        name: "FK_roads_difficulties_difficultyID",
                        column: x => x.difficultyID,
                        principalTable: "difficulties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roads_regions_regionId",
                        column: x => x.regionId,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "difficulties",
                columns: new[] { "id", "Name" },
                values: new object[,]
                {
                    { new Guid("ddddddd1-dddd-dddd-dddd-ddddddddddd1"), "Hard" },
                    { new Guid("ddddddd2-dddd-dddd-dddd-ddddddddddd2"), "Easy" },
                    { new Guid("ddddddd3-dddd-dddd-dddd-ddddddddddd3"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "regions",
                columns: new[] { "id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "RG-001", "Northern Highlands", "https://example.com/images/northern_hills.jpg" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "RG-002", "Desert Plains", "https://example.com/images/desert_plains.jpg" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "RG-003", "Coastal Edge", "https://example.com/images/coastal_edge.jpg" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "RG-004", "Forest Path", "https://example.com/images/forest_path.jpg" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "RG-005", "Urban Loop", "https://example.com/images/urban_loop.jpg" }
                });

            migrationBuilder.InsertData(
                table: "roads",
                columns: new[] { "id", "Description", "LengthInKm", "Name", "RoadImageUrl", "difficultyID", "regionId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), "A scenic mountain road with sharp curves.", 12.5, "Hilltop Pass", "https://example.com/images/hilltop_pass.jpg", new Guid("ddddddd2-dddd-dddd-dddd-ddddddddddd2"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), "A flat road through the desert, good for beginners.", 25.0, "Sandy Trail", "https://example.com/images/sandy_trail.jpg", new Guid("ddddddd2-dddd-dddd-dddd-ddddddddddd2"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Beautiful road along the coastline with sea views.", 18.199999999999999, "Ocean Drive", "https://example.com/images/ocean_drive.jpg", new Guid("ddddddd2-dddd-dddd-dddd-ddddddddddd2"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("aaaaaaa4-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), "A quiet road through dense pine forests.", 9.6999999999999993, "Pinewood Route", "https://example.com/images/pinewood_route.jpg", new Guid("ddddddd3-dddd-dddd-dddd-ddddddddddd3"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("aaaaaaa5-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), "Circular road inside the city, heavy traffic.", 5.2999999999999998, "City Circuit", "https://example.com/images/city_circuit.jpg", new Guid("ddddddd1-dddd-dddd-dddd-ddddddddddd1"), new Guid("55555555-5555-5555-5555-555555555555") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_roads_difficultyID",
                table: "roads",
                column: "difficultyID");

            migrationBuilder.CreateIndex(
                name: "IX_roads_regionId",
                table: "roads",
                column: "regionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roads");

            migrationBuilder.DropTable(
                name: "difficulties");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
