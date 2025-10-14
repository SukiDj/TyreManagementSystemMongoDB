using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProdSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerUnit",
                table: "Sales",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionId",
                table: "Sales",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "Sales",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TargetMarket",
                table: "Sales",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "Sales",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductionId",
                table: "Sales",
                column: "ProductionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Productions_ProductionId",
                table: "Sales",
                column: "ProductionId",
                principalTable: "Productions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Productions_ProductionId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ProductionId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductionId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TargetMarket",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "Sales");
        }
    }
}
