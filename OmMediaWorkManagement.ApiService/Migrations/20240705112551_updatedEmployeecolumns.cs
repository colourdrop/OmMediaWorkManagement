using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updatedEmployeecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppPin",
                table: "OmEmployee",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "OmEmployee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTPAttempts",
                table: "OmEmployee",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpireTime",
                table: "OmEmployee",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPGeneratedTime",
                table: "OmEmployee",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppPin",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "OTPAttempts",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "OTPExpireTime",
                table: "OmEmployee");

            migrationBuilder.DropColumn(
                name: "OTPGeneratedTime",
                table: "OmEmployee");
        }
    }
}
