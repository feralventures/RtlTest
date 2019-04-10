using Microsoft.EntityFrameworkCore.Migrations;

namespace RtlTestRepository.Migrations
{
    public partial class Optimizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Show_TvMazeId",
                table: "Show",
                column: "TvMazeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Show_TvMazeId",
                table: "Show");
        }
    }
}
