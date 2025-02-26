using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskIcosoftBackend.Migrations
{
    /// <inheritdoc />
    public partial class NewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_SessionType_IdSessionType",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionType",
                table: "SessionType");

            migrationBuilder.RenameTable(
                name: "SessionType",
                newName: "SessionTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionTypes",
                table: "SessionTypes",
                column: "IdSessionType");

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    IdCompany = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyFiscalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyComercialName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdCart = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.IdCompany);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_SessionTypes_IdSessionType",
                table: "Sessions",
                column: "IdSessionType",
                principalTable: "SessionTypes",
                principalColumn: "IdSessionType",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_SessionTypes_IdSessionType",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionTypes",
                table: "SessionTypes");

            migrationBuilder.RenameTable(
                name: "SessionTypes",
                newName: "SessionType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionType",
                table: "SessionType",
                column: "IdSessionType");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_SessionType_IdSessionType",
                table: "Sessions",
                column: "IdSessionType",
                principalTable: "SessionType",
                principalColumn: "IdSessionType",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
