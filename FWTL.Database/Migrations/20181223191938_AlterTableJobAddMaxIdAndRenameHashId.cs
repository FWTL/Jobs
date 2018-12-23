using Microsoft.EntityFrameworkCore.Migrations;

namespace FWTL.Database.Migrations
{
    public partial class AlterTableJobAddMaxIdAndRenameHashId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneHashId",
                table: "Job",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_PhoneHashId",
                table: "Job",
                newName: "IX_Job_UserId");

            migrationBuilder.AddColumn<int>(
                name: "MaxId",
                table: "Job",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxId",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Job",
                newName: "PhoneHashId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_UserId",
                table: "Job",
                newName: "IX_Job_PhoneHashId");
        }
    }
}
