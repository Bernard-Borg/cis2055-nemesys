using Microsoft.EntityFrameworkCore.Migrations;

namespace Nemesys.Migrations
{
    public partial class UpdatedUserIdtostringinInvestigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_AspNetUsers_InvestigatorId",
                table: "Investigations");

            migrationBuilder.DropIndex(
                name: "IX_Investigations_InvestigatorId",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "InvestigatorId",
                table: "Investigations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Investigations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_UserId",
                table: "Investigations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_AspNetUsers_UserId",
                table: "Investigations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investigations_AspNetUsers_UserId",
                table: "Investigations");

            migrationBuilder.DropIndex(
                name: "IX_Investigations_UserId",
                table: "Investigations");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Investigations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvestigatorId",
                table: "Investigations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_InvestigatorId",
                table: "Investigations",
                column: "InvestigatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investigations_AspNetUsers_InvestigatorId",
                table: "Investigations",
                column: "InvestigatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
