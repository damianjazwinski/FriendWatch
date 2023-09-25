using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendWatch.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExpirationDateToWatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Watches",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Watches");
        }
    }
}
