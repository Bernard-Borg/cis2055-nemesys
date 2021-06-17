using Microsoft.EntityFrameworkCore.Migrations;

namespace Nemesys.Migrations
{
    public partial class Addedhazardtypesreportstatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Reports",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "HazardType",
                table: "Reports",
                newName: "HazardTypeId");

            migrationBuilder.CreateTable(
                name: "HazardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HazardName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HazardTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HexColour = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_HazardTypeId",
                table: "Reports",
                column: "HazardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StatusId",
                table: "Reports",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_HazardTypes_HazardTypeId",
                table: "Reports",
                column: "HazardTypeId",
                principalTable: "HazardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportStatuses_StatusId",
                table: "Reports",
                column: "StatusId",
                principalTable: "ReportStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_HazardTypes_HazardTypeId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportStatuses_StatusId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "HazardTypes");

            migrationBuilder.DropTable(
                name: "ReportStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Reports_HazardTypeId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_StatusId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Reports",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "HazardTypeId",
                table: "Reports",
                newName: "HazardType");
        }
    }
}
