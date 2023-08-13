using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Migrations
{
    /// <inheritdoc />
    public partial class CirclesToUsersRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Circles_CircleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CircleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CircleId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "CircleUser",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "int", nullable: false),
                    CirclesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleUser", x => new { x.MembersId, x.CirclesId });
                    table.ForeignKey(
                        name: "FK_CircleUser_Circles_CirclesId",
                        column: x => x.CirclesId,
                        principalTable: "Circles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CircleUser_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CircleUser_CirclesId",
                table: "CircleUser",
                column: "CirclesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircleUser");

            migrationBuilder.AddColumn<int>(
                name: "CircleId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CircleId",
                table: "Users",
                column: "CircleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Circles_CircleId",
                table: "Users",
                column: "CircleId",
                principalTable: "Circles",
                principalColumn: "Id");
        }
    }
}
