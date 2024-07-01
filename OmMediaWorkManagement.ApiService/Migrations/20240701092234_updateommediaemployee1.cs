using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updateommediaemployee1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryAmount",
                table: "OmEmployeeSalary");

            migrationBuilder.DropColumn(
                name: "AdvancePayment",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "DueBalance",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "AdvancePaymentDate",
                table: "OmEmployeeSalary",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "OmEmployeeSalary",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OverBalance",
                table: "OmEmployeeSalary",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OverTimeSalary",
                table: "OmEmployeeSalary",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSalaryPaid",
                table: "OmEmployee",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryAmount",
                table: "OmEmployee",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancePaymentDate",
                table: "OmEmployeeSalary");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OmEmployeeSalary");

            migrationBuilder.DropColumn(
                name: "OverBalance",
                table: "OmEmployeeSalary");

            migrationBuilder.DropColumn(
                name: "OverTimeSalary",
                table: "OmEmployeeSalary");

            migrationBuilder.DropColumn(
                name: "IsSalaryPaid",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "SalaryAmount",
                table: "OmEmployee");

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryAmount",
                table: "OmEmployeeSalary",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AdvancePayment",
                table: "OmEmployee",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DueBalance",
                table: "OmEmployee",
                type: "numeric",
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
        }
    }
}
