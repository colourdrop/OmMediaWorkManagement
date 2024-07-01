using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updateommediaemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OmEmployee",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AdvancePayment",
                table: "OmEmployee",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "OmEmployee",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DueBalance",
                table: "OmEmployee",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeProfilePath",
                table: "OmEmployee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "OmEmployee",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftOverTime",
                table: "OmEmployee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftTiming",
                table: "OmEmployee",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OmEmployeeDocuments",
                columns: table => new
                {
                    OmEmployeeDocumentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OmEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeDocumentsPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmEmployeeDocuments", x => x.OmEmployeeDocumentId);
                    table.ForeignKey(
                        name: "FK_OmEmployeeDocuments_OmEmployee_OmEmployeeId",
                        column: x => x.OmEmployeeId,
                        principalTable: "OmEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OmEmployeeSalary",
                columns: table => new
                {
                    EmployeeSalaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OmEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    SalaryAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    AdvancePayment = table.Column<decimal>(type: "numeric", nullable: true),
                    DueBalance = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmEmployeeSalary", x => x.EmployeeSalaryId);
                    table.ForeignKey(
                        name: "FK_OmEmployeeSalary_OmEmployee_OmEmployeeId",
                        column: x => x.OmEmployeeId,
                        principalTable: "OmEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OmEmployeeShift",
                columns: table => new
                {
                    EmployeeShiftId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OmEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ShiftTiming = table.Column<string>(type: "text", nullable: false),
                    ShiftOverTime = table.Column<string>(type: "text", nullable: true),
                    OverTimePerHourCost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmEmployeeShift", x => x.EmployeeShiftId);
                    table.ForeignKey(
                        name: "FK_OmEmployeeShift_OmEmployee_OmEmployeeId",
                        column: x => x.OmEmployeeId,
                        principalTable: "OmEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OmEmployeeDocuments_OmEmployeeId",
                table: "OmEmployeeDocuments",
                column: "OmEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OmEmployeeSalary_OmEmployeeId",
                table: "OmEmployeeSalary",
                column: "OmEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OmEmployeeShift_OmEmployeeId",
                table: "OmEmployeeShift",
                column: "OmEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OmEmployeeDocuments");

            migrationBuilder.DropTable(
                name: "OmEmployeeSalary");

            migrationBuilder.DropTable(
                name: "OmEmployeeShift");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "AdvancePayment",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "DueBalance",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "EmployeeProfilePath",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "ShiftOverTime",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "ShiftTiming",
                table: "OmEmployee");
        }
    }
}
