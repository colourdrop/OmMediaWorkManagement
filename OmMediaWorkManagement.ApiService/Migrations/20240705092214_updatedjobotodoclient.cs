using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updatedjobotodoclient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "JobToDo");

            migrationBuilder.AddColumn<int>(
                name: "OmClientId",
                table: "JobToDo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobToDo_OmClientId",
                table: "JobToDo",
                column: "OmClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobToDo_OmClient_OmClientId",
                table: "JobToDo",
                column: "OmClientId",
                principalTable: "OmClient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobToDo_OmClient_OmClientId",
                table: "JobToDo");

            migrationBuilder.DropIndex(
                name: "IX_JobToDo_OmClientId",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "OmClientId",
                table: "JobToDo");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "JobToDo",
                type: "text",
                nullable: true);
        }
    }
}
