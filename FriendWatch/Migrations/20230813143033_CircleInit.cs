using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Migrations
{
    /// <inheritdoc />
    public partial class CircleInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CircleId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Circles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Circles_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CircleId",
                table: "Users",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_Circles_OwnerId",
                table: "Circles",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Circles_CircleId",
                table: "Users",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Circles_CircleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Circles");

            migrationBuilder.DropIndex(
                name: "IX_Users_CircleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CircleId",
                table: "Users");
        }
    }
}
