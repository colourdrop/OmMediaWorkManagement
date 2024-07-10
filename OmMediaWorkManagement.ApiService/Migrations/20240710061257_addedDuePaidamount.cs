using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addedDuePaidamount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DueBalance",
                table: "OmClientWork",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaidAmount",
                table: "OmClientWork",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DueBalance",
                table: "JobToDo",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaidAmount",
                table: "JobToDo",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueBalance",
                table: "OmClientWork");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "OmClientWork");

            migrationBuilder.DropColumn(
                name: "DueBalance",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "JobToDo");
        }
    }
}
