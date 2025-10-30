using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpirationDateTime",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokenOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.ApplicationUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "difficulties",
                keyColumn: "id",
                keyValue: new Guid("ddddddd1-dddd-dddd-dddd-ddddddddddd1"),
                column: "Name",
                value: "Easy");

            migrationBuilder.UpdateData(
                table: "difficulties",
                keyColumn: "id",
                keyValue: new Guid("ddddddd2-dddd-dddd-dddd-ddddddddddd2"),
                column: "Name",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "difficulties",
                keyColumn: "id",
                keyValue: new Guid("ddddddd3-dddd-dddd-dddd-ddddddddddd3"),
                column: "Name",
                value: "Hard");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpirationDateTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "difficulties",
                keyColumn: "id",
                keyValue: new Guid("ddddddd1-dddd-dddd-dddd-ddddddddddd1"),
                column: "Name",
                value: "Hard");

            migrationBuilder.UpdateData(
                table: "difficulties",
                keyColumn: "id",
                keyValue: new Guid("ddddddd2-dddd-dddd-dddd-ddddddddddd2"),
                column: "Name",
                value: "Easy");

            migrationBuilder.UpdateData(
                table: "difficulties",
                keyColumn: "id",
                keyValue: new Guid("ddddddd3-dddd-dddd-dddd-ddddddddddd3"),
                column: "Name",
                value: "Medium");
        }
    }
}
