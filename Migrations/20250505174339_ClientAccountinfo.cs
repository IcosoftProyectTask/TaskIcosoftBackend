using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskIcosoftBackend.Migrations
{
    /// <inheritdoc />
    public partial class ClientAccountinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hotmail",
                table: "ClienteAccountInfos");

            migrationBuilder.DropColumn(
                name: "HotmailPassword",
                table: "ClienteAccountInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hotmail",
                table: "ClienteAccountInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HotmailPassword",
                table: "ClienteAccountInfos",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
