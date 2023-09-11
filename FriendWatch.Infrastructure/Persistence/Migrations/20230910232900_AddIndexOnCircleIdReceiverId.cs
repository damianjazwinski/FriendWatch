using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOnCircleIdReceiverId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invitations_CircleId",
                table: "Invitations");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CircleId_ReceiverId",
                table: "Invitations",
                columns: new[] { "CircleId", "ReceiverId" },
                unique: true,
                filter: "[ReceiverId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invitations_CircleId_ReceiverId",
                table: "Invitations");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CircleId",
                table: "Invitations",
                column: "CircleId");
        }
    }
}
