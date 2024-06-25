using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updatedTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "JobToDo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "JobToDo",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total",
                table: "JobToDo",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "total",
                table: "JobToDo");
        }
    }
}
