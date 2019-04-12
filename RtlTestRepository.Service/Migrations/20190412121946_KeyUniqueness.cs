using Microsoft.EntityFrameworkCore.Migrations;

namespace RtlTestRepository.Migrations
{
    public partial class KeyUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Show_TvMazeId",
                table: "Show");

            migrationBuilder.CreateIndex(
                name: "IX_Show_TvMazeId",
                table: "Show",
                column: "TvMazeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Show_TvMazeId",
                table: "Show");

            migrationBuilder.CreateIndex(
                name: "IX_Show_TvMazeId",
                table: "Show",
                column: "TvMazeId");
        }
    }
}
