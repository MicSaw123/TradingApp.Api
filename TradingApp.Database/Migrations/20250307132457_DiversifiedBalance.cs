using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class DiversifiedBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "SpotPortfolios",
                newName: "DisposableBalance");

            migrationBuilder.RenameColumn(
                name: "AllTransactionsWorth",
                table: "SpotPortfolios",
                newName: "AllocatedBalance");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Portfolios",
                newName: "DisposableBalance");

            migrationBuilder.RenameColumn(
                name: "AllTransactionsWorth",
                table: "Portfolios",
                newName: "AllocatedBalance");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "FuturesPortfolios",
                newName: "DisposableBalance");

            migrationBuilder.RenameColumn(
                name: "AllTransactionsWorth",
                table: "FuturesPortfolios",
                newName: "AllocatedBalance");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastEditTransactionDate",
                table: "SpotTransactions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastEditTransactionDate",
                table: "FuturesTransactions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisposableBalance",
                table: "SpotPortfolios",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "AllocatedBalance",
                table: "SpotPortfolios",
                newName: "AllTransactionsWorth");

            migrationBuilder.RenameColumn(
                name: "DisposableBalance",
                table: "Portfolios",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "AllocatedBalance",
                table: "Portfolios",
                newName: "AllTransactionsWorth");

            migrationBuilder.RenameColumn(
                name: "DisposableBalance",
                table: "FuturesPortfolios",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "AllocatedBalance",
                table: "FuturesPortfolios",
                newName: "AllTransactionsWorth");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastEditTransactionDate",
                table: "SpotTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastEditTransactionDate",
                table: "FuturesTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
