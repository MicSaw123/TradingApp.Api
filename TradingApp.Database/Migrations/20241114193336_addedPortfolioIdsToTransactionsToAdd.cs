using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedPortfolioIdsToTransactionsToAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpotPortfolioId",
                table: "SpotTransactionsToOpen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FuturesPortfolioId",
                table: "FuturesTransactionsToOpen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpotTransactionsToOpen_SpotPortfolioId",
                table: "SpotTransactionsToOpen",
                column: "SpotPortfolioId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SpotTransactionsToOpen_SpotPortfolios_SpotPortfolioId",
                table: "SpotTransactionsToOpen",
                column: "SpotPortfolioId",
                principalTable: "SpotPortfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuturesTransactionsToOpen_FuturesPortfolios_FuturesPortfolioId",
                table: "FuturesTransactionsToOpen");

            migrationBuilder.DropForeignKey(
                name: "FK_SpotTransactionsToOpen_SpotPortfolios_SpotPortfolioId",
                table: "SpotTransactionsToOpen");

            migrationBuilder.DropIndex(
                name: "IX_SpotTransactionsToOpen_SpotPortfolioId",
                table: "SpotTransactionsToOpen");

            migrationBuilder.DropIndex(
                name: "IX_FuturesTransactionsToOpen_FuturesPortfolioId",
                table: "FuturesTransactionsToOpen");

            migrationBuilder.DropColumn(
                name: "SpotPortfolioId",
                table: "SpotTransactionsToOpen");

            migrationBuilder.DropColumn(
                name: "FuturesPortfolioId",
                table: "FuturesTransactionsToOpen");
        }
    }
}
