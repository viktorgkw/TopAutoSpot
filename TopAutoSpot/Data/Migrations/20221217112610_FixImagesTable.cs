using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImages_Cars_CarId",
                table: "VehicleImages");

            migrationBuilder.DropIndex(
                name: "IX_VehicleImages_CarId",
                table: "VehicleImages");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "VehicleImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarId",
                table: "VehicleImages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImages_CarId",
                table: "VehicleImages",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImages_Cars_CarId",
                table: "VehicleImages",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }
    }
}
