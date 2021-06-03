using Microsoft.EntityFrameworkCore.Migrations;

namespace Nemesys.Migrations
{
    public partial class Investigationnavigationproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investigations_ReportId",
                table: "Investigations");

            migrationBuilder.AddColumn<int>(
                name: "InvestigationId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_ReportId",
                table: "Investigations",
                column: "ReportId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investigations_ReportId",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "InvestigationId",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_ReportId",
                table: "Investigations",
                column: "ReportId");
        }
    }
}
