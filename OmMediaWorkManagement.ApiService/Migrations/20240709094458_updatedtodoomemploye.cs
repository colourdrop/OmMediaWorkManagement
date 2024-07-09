using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updatedtodoomemploye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobToDo");

            migrationBuilder.AddColumn<int>(
                name: "OmEmpId",
                table: "JobToDo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobToDo_OmEmpId",
                table: "JobToDo",
                column: "OmEmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobToDo_OmEmployee_OmEmpId",
                table: "JobToDo",
                column: "OmEmpId",
                principalTable: "OmEmployee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobToDo_OmEmployee_OmEmpId",
                table: "JobToDo");

            migrationBuilder.DropIndex(
                name: "IX_JobToDo_OmEmpId",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "OmEmpId",
                table: "JobToDo");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JobToDo",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
