using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addedimageoath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "JobImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "JobImages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "JobImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "JobImages",
                type: "bytea",
                nullable: true);
        }
    }
}
