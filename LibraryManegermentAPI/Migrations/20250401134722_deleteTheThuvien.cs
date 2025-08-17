using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManegermentAPI.Migrations
{
    /// <inheritdoc />
    public partial class deleteTheThuvien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheThuViens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TheThuViens",
                columns: table => new
                {
                    MaThe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDocGia = table.Column<int>(type: "int", nullable: false),
                    NgayCap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheThuViens", x => x.MaThe);
                    table.ForeignKey(
                        name: "FK_TheThuViens_DocGias_MaDocGia",
                        column: x => x.MaDocGia,
                        principalTable: "DocGias",
                        principalColumn: "MaDocGia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheThuViens_MaDocGia",
                table: "TheThuViens",
                column: "MaDocGia");
        }
    }
}
