using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskIcosoftBackend.Migrations
{
    /// <inheritdoc />
    public partial class License : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorAccount",
                table: "Licenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorAccount",
                table: "Licenses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
