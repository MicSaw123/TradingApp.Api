using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class spotTransactionsToClose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotTransactionsToOpen_SpotPortfolios_SpotPortfolioId",
                table: "SpotTransactionsToOpen");

            migrationBuilder.DropIndex(
                name: "IX_SpotTransactionsToOpen_SpotPortfolioId",
                table: "SpotTransactionsToOpen");

            migrationBuilder.AddColumn<float>(
                name: "SellingPrice",
                table: "SpotTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SellingPrice",
                table: "FuturesTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "SpotTransactions");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "FuturesTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_SpotTransactionsToOpen_SpotPortfolioId",
                table: "SpotTransactionsToOpen",
                column: "SpotPortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotTransactionsToOpen_SpotPortfolios_SpotPortfolioId",
                table: "SpotTransactionsToOpen",
                column: "SpotPortfolioId",
                principalTable: "SpotPortfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
