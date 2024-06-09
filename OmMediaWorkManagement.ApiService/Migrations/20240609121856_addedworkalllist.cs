using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addedworkalllist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "JobToDo",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "JobToDo");
        }
    }
}
