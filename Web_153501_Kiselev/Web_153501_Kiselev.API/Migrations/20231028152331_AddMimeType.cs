using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_153501_Kiselev.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "Vehicles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "Vehicles");
        }
    }
}
