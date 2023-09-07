using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFileToCircle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageFileId",
                table: "Circles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Circles_ImageFileId",
                table: "Circles",
                column: "ImageFileId",
                unique: true,
                filter: "[ImageFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Circles_ImageFiles_ImageFileId",
                table: "Circles",
                column: "ImageFileId",
                principalTable: "ImageFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circles_ImageFiles_ImageFileId",
                table: "Circles");

            migrationBuilder.DropIndex(
                name: "IX_Circles_ImageFileId",
                table: "Circles");

            migrationBuilder.DropColumn(
                name: "ImageFileId",
                table: "Circles");
        }
    }
}
