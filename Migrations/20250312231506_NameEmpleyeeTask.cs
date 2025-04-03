using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskIcosoftBackend.Migrations
{
    /// <inheritdoc />
    public partial class NameEmpleyeeTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTasks_CompanyEmployees_IdCompanyEmployee",
                table: "SupportTasks");

            migrationBuilder.DropIndex(
                name: "IX_SupportTasks_IdCompanyEmployee",
                table: "SupportTasks");

            migrationBuilder.DropColumn(
                name: "IdCompanyEmployee",
                table: "SupportTasks");

            migrationBuilder.AddColumn<string>(
                name: "NameEmployeeCompany",
                table: "SupportTasks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEmployeeCompany",
                table: "SupportTasks");

            migrationBuilder.AddColumn<int>(
                name: "IdCompanyEmployee",
                table: "SupportTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTasks_IdCompanyEmployee",
                table: "SupportTasks",
                column: "IdCompanyEmployee");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTasks_CompanyEmployees_IdCompanyEmployee",
                table: "SupportTasks",
                column: "IdCompanyEmployee",
                principalTable: "CompanyEmployees",
                principalColumn: "IdCompanyEmployee",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
