using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullCreatedAtCircle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Circles");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Circles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Circles");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Circles",
                type: "datetime2",
                nullable: true);
        }
    }
}
