using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AfterRefactorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "SpotTransactions",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "FuturesTransactions",
                newName: "IsActive");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosingTransactionDate",
                table: "SpotTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<float>(
                name: "CurrentValue",
                table: "SpotTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TodaysProfit",
                table: "SpotTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AllTransactionsWorth",
                table: "SpotPortfolios",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SpotPortfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "AllTransactionsWorth",
                table: "Portfolios",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosingTransactionDate",
                table: "FuturesTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "FuturesPortfolioId",
                table: "FuturesTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TodaysProfit",
                table: "FuturesTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AllTransactionsWorth",
                table: "FuturesPortfolios",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FuturesPortfolios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingTransactionDate",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "TodaysProfit",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "AllTransactionsWorth",
                table: "SpotPortfolios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SpotPortfolios");

            migrationBuilder.DropColumn(
                name: "AllTransactionsWorth",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "ClosingTransactionDate",
                table: "FuturesTransactions");

            migrationBuilder.DropColumn(
                name: "FuturesPortfolioId",
                table: "FuturesTransactions");

            migrationBuilder.DropColumn(
                name: "TodaysProfit",
                table: "FuturesTransactions");

            migrationBuilder.DropColumn(
                name: "AllTransactionsWorth",
                table: "FuturesPortfolios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FuturesPortfolios");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "SpotTransactions",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "FuturesTransactions",
                newName: "isActive");
        }
    }
}
