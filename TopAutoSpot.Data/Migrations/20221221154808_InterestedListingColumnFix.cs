using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class InterestedListingColumnFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestedListing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterestedListing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VehicleCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestedListing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestedListing_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestedListing_UserId",
                table: "InterestedListing",
                column: "UserId");
        }
    }
}
