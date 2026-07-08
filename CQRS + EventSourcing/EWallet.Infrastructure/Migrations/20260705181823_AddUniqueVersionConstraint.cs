using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueVersionConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventStore_EntityId_Version",
                table: "EventStore");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "EventStore",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_EventStore_EntityId_Version",
                table: "EventStore",
                columns: new[] { "EntityId", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventStore_EntityId_Version",
                table: "EventStore");

            migrationBuilder.AlterColumn<string>(
                name: "EntityId",
                table: "EventStore",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_EventStore_EntityId_Version",
                table: "EventStore",
                columns: new[] { "EntityId", "Version" });
        }
    }
}
