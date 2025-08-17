using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManegermentAPI.Migrations
{
    /// <inheritdoc />
    public partial class commit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocGias_AspNetUsers_ApplicationUserId",
                table: "DocGias");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_AspNetUsers_ApplicationUserId",
                table: "NhanViens");

            migrationBuilder.DropIndex(
                name: "IX_NhanViens_ApplicationUserId",
                table: "NhanViens");

            migrationBuilder.DropIndex(
                name: "IX_DocGias_ApplicationUserId",
                table: "DocGias");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "NhanViens");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DocGias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "NhanViens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "DocGias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_ApplicationUserId",
                table: "NhanViens",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocGias_ApplicationUserId",
                table: "DocGias",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DocGias_AspNetUsers_ApplicationUserId",
                table: "DocGias",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_AspNetUsers_ApplicationUserId",
                table: "NhanViens",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
