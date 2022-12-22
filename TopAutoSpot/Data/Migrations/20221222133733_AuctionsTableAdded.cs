using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AuctionsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuctionId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartHour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuctioneerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AuctionId",
                table: "AspNetUsers",
                column: "AuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Auctions_AuctionId",
                table: "AspNetUsers",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Auctions_AuctionId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AuctionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AuctionId",
                table: "AspNetUsers");
        }
    }
}
