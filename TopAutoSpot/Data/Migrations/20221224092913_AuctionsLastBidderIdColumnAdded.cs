using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AuctionsLastBidderIdColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastBidderId",
                table: "Auctions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastBidderId",
                table: "Auctions");
        }
    }
}
