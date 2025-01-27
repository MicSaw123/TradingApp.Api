using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCoinFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_AspNetUsers_TradingAppUserId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_FuturesPortfolios_FuturesPortfolioId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_SpotPortfolios_SpotPortfolioId",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_FuturesPortfolioId",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_SpotPortfolioId",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_TradingAppUserId",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "TradingAppUserId",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "FuturesTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsShort",
                table: "FuturesTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<float>(
                name: "AllTimeLowPrice",
                table: "Coins",
                type: "real",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<float>(
                name: "AllTimeHighPrice",
                table: "Coins",
                type: "real",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShort",
                table: "FuturesTransactions");

            migrationBuilder.AddColumn<string>(
                name: "TradingAppUserId",
                table: "Portfolios",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "SellingPrice",
                table: "FuturesTransactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<string>(
                name: "AllTimeLowPrice",
                table: "Coins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "AllTimeHighPrice",
                table: "Coins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_FuturesPortfolioId",
                table: "Portfolios",
                column: "FuturesPortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_SpotPortfolioId",
                table: "Portfolios",
                column: "SpotPortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_TradingAppUserId",
                table: "Portfolios",
                column: "TradingAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_AspNetUsers_TradingAppUserId",
                table: "Portfolios",
                column: "TradingAppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_FuturesPortfolios_FuturesPortfolioId",
                table: "Portfolios",
                column: "FuturesPortfolioId",
                principalTable: "FuturesPortfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_SpotPortfolios_SpotPortfolioId",
                table: "Portfolios",
                column: "SpotPortfolioId",
                principalTable: "SpotPortfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
