using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updatedEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OmEmployeeSalary");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "OmEmployee");

            migrationBuilder.CreateTable(
                name: "OmEmployeeSalaryManagement",
                columns: table => new
                {
                    EmployeeSalaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OmEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    AdvancePayment = table.Column<decimal>(type: "numeric", nullable: true),
                    AdvancePaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OverTimeSalary = table.Column<decimal>(type: "numeric", nullable: true),
                    DueBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    OverBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OmEmployeeSalaryManagement", x => x.EmployeeSalaryId);
                    table.ForeignKey(
                        name: "FK_OmEmployeeSalaryManagement_OmEmployee_OmEmployeeId",
                        column: x => x.OmEmployeeId,
                        principalTable: "OmEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OmEmployeeSalaryManagement_OmEmployeeId",
                table: "OmEmployeeSalaryManagement",
                column: "OmEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OmEmployeeSalaryManagement");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "OmEmployee",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "OmEmployeeSalary",
                columns: table => new
                {
                    EmployeeSalaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OmEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    AdvancePayment = table.Column<decimal>(type: "numeric", nullable: true),
                    AdvancePaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    OverBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    OverTimeSalary = table.Column<decimal>(type: "numeric", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_OmEmployeeSalary_OmEmployeeId",
                table: "OmEmployeeSalary",
                column: "OmEmployeeId");
        }
    }
}
