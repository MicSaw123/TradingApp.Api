using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class userIdRemovedFromPortfolios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SpotPortfolios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FuturesPortfolios");

            migrationBuilder.RenameColumn(
                name: "TodaysProfit",
                table: "SpotTransactions",
                newName: "CurrentTransactionWorth");

            migrationBuilder.RenameColumn(
                name: "TodaysProfit",
                table: "FuturesTransactions",
                newName: "CurrentTransactionWorth");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastEditTransactionDate",
                table: "SpotTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "OpenTransactionDate",
                table: "SpotTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastEditTransactionDate",
                table: "FuturesTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "OpenTransactionDate",
                table: "FuturesTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditTransactionDate",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "OpenTransactionDate",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "LastEditTransactionDate",
                table: "FuturesTransactions");

            migrationBuilder.DropColumn(
                name: "OpenTransactionDate",
                table: "FuturesTransactions");

            migrationBuilder.RenameColumn(
                name: "CurrentTransactionWorth",
                table: "SpotTransactions",
                newName: "TodaysProfit");

            migrationBuilder.RenameColumn(
                name: "CurrentTransactionWorth",
                table: "FuturesTransactions",
                newName: "TodaysProfit");

            migrationBuilder.AddColumn<float>(
                name: "CurrentValue",
                table: "SpotTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SpotPortfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FuturesPortfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
