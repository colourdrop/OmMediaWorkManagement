using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addedjobtypestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobIsDeclained",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "JobIsFinished",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "JobIsHold",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "JobIsRunning",
                table: "JobToDo");

            migrationBuilder.AddColumn<bool>(
                name: "IsStatus",
                table: "JobToDo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JobStatusType",
                table: "JobToDo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobTypeStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobStatusType = table.Column<int>(type: "integer", nullable: false),
                    JobStatusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypeStatus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobTypeStatus");

            migrationBuilder.DropColumn(
                name: "IsStatus",
                table: "JobToDo");

            migrationBuilder.DropColumn(
                name: "JobStatusType",
                table: "JobToDo");

            migrationBuilder.AddColumn<bool>(
                name: "JobIsDeclained",
                table: "JobToDo",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "JobIsFinished",
                table: "JobToDo",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "JobIsHold",
                table: "JobToDo",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "JobIsRunning",
                table: "JobToDo",
                type: "boolean",
                nullable: true);
        }
    }
}
