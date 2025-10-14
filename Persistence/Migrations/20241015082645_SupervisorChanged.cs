using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SupervisorChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Productions_ProductionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProductionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProdOrderID",
                table: "Productions");

            migrationBuilder.DropColumn(
                name: "ProductionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "Productions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Expires = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Revoked = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productions_SupervisorId",
                table: "Productions",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productions_AspNetUsers_SupervisorId",
                table: "Productions",
                column: "SupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productions_AspNetUsers_SupervisorId",
                table: "Productions");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Productions_SupervisorId",
                table: "Productions");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Productions");

            migrationBuilder.AddColumn<int>(
                name: "ProdOrderID",
                table: "Productions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProductionId",
                table: "AspNetUsers",
                column: "ProductionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Productions_ProductionId",
                table: "AspNetUsers",
                column: "ProductionId",
                principalTable: "Productions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
