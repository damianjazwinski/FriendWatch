using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNullabilityInvitationReceiverId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invitations_CircleId_ReceiverId",
                table: "Invitations");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "Invitations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CircleId_ReceiverId",
                table: "Invitations",
                columns: new[] { "CircleId", "ReceiverId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invitations_CircleId_ReceiverId",
                table: "Invitations");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "Invitations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CircleId_ReceiverId",
                table: "Invitations",
                columns: new[] { "CircleId", "ReceiverId" },
                unique: true,
                filter: "[ReceiverId] IS NOT NULL");
        }
    }
}
