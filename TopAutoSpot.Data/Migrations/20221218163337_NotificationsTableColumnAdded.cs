using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopAutoSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationsTableColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "To",
                table: "Notifications");
        }
    }
}
