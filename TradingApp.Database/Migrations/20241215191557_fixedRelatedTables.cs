using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class fixedRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuturesTransactionsToOpen_FuturesPortfolios_FuturesPortfolioId",
                table: "FuturesTransactionsToOpen");

            migrationBuilder.DropIndex(
                name: "IX_FuturesTransactionsToOpen_FuturesPortfolioId",
                table: "FuturesTransactionsToOpen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FuturesTransactionsToOpen_FuturesPortfolioId",
                table: "FuturesTransactionsToOpen",
                column: "FuturesPortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuturesTransactionsToOpen_FuturesPortfolios_FuturesPortfolioId",
                table: "FuturesTransactionsToOpen",
                column: "FuturesPortfolioId",
                principalTable: "FuturesPortfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
