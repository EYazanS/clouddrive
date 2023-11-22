using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserWithDataAndNotebooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPasswords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notebooks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Data",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPasswords_UserId",
                table: "UserPasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_UserId",
                table: "Notebooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_UserId",
                table: "Data",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_AspNetUsers_UserId",
                table: "Data",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notebooks_AspNetUsers_UserId",
                table: "Notebooks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswords_AspNetUsers_UserId",
                table: "UserPasswords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_AspNetUsers_UserId",
                table: "Data");

            migrationBuilder.DropForeignKey(
                name: "FK_Notebooks_AspNetUsers_UserId",
                table: "Notebooks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswords_AspNetUsers_UserId",
                table: "UserPasswords");

            migrationBuilder.DropIndex(
                name: "IX_UserPasswords_UserId",
                table: "UserPasswords");

            migrationBuilder.DropIndex(
                name: "IX_Notebooks_UserId",
                table: "Notebooks");

            migrationBuilder.DropIndex(
                name: "IX_Data_UserId",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notebooks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Data");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserPasswords",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
