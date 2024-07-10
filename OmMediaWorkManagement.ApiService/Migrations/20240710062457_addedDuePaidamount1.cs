using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addedDuePaidamount1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPayable",
                table: "OmClientWork",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPayable",
                table: "JobToDo",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPayable",
                table: "OmClientWork");

            migrationBuilder.DropColumn(
                name: "TotalPayable",
                table: "JobToDo");
        }
    }
}
