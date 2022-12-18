using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class VehiclesStatusAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Trucks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Trailers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Motorcycles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Boats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Boats");
        }
    }
}
