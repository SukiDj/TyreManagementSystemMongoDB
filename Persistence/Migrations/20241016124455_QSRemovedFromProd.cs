using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class QSRemovedFromProd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productions_AspNetUsers_SupervisorId",
                table: "Productions");

            migrationBuilder.DropIndex(
                name: "IX_Productions_SupervisorId",
                table: "Productions");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Productions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "Productions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productions_SupervisorId",
                table: "Productions",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productions_AspNetUsers_SupervisorId",
                table: "Productions",
                column: "SupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
