using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmMediaWorkManagement.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class updatedUserIdidentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OmEmployee_UserId",
                table: "OmEmployee",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OmClientWork_UserId",
                table: "OmClientWork",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OmClient_UserId",
                table: "OmClient",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OmClient_AspNetUsers_UserId",
                table: "OmClient",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OmClientWork_AspNetUsers_UserId",
                table: "OmClientWork",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OmEmployee_AspNetUsers_UserId",
                table: "OmEmployee",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmClient_AspNetUsers_UserId",
                table: "OmClient");

            migrationBuilder.DropForeignKey(
                name: "FK_OmClientWork_AspNetUsers_UserId",
                table: "OmClientWork");

            migrationBuilder.DropForeignKey(
                name: "FK_OmEmployee_AspNetUsers_UserId",
                table: "OmEmployee");

            migrationBuilder.DropIndex(
                name: "IX_OmEmployee_UserId",
                table: "OmEmployee");

            migrationBuilder.DropIndex(
                name: "IX_OmClientWork_UserId",
                table: "OmClientWork");

            migrationBuilder.DropIndex(
                name: "IX_OmClient_UserId",
                table: "OmClient");
        }
    }
}
