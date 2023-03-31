using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class TypoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManufactoreDate",
                table: "Trucks",
                newName: "ManufactureDate");

            migrationBuilder.RenameColumn(
                name: "ManufactoreDate",
                table: "Trailers",
                newName: "ManufactureDate");

            migrationBuilder.RenameColumn(
                name: "ManufactoreDate",
                table: "Motorcycles",
                newName: "ManufactureDate");

            migrationBuilder.RenameColumn(
                name: "ManufactoreDate",
                table: "Cars",
                newName: "ManufactureDate");

            migrationBuilder.RenameColumn(
                name: "ManufactoreDate",
                table: "Buses",
                newName: "ManufactureDate");

            migrationBuilder.RenameColumn(
                name: "ManufactoreDate",
                table: "Boats",
                newName: "ManufactureDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Trucks",
                newName: "ManufactoreDate");

            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Trailers",
                newName: "ManufactoreDate");

            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Motorcycles",
                newName: "ManufactoreDate");

            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Cars",
                newName: "ManufactoreDate");

            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Buses",
                newName: "ManufactoreDate");

            migrationBuilder.RenameColumn(
                name: "ManufactureDate",
                table: "Boats",
                newName: "ManufactoreDate");
        }
    }
}
