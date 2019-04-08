using Microsoft.EntityFrameworkCore.Migrations;

namespace RtlTestRepository.Migrations
{
    public partial class AddedTvMazeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TvMazeId",
                table: "Show",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TvMazeId",
                table: "Person",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TvMazeId",
                table: "Show");

            migrationBuilder.DropColumn(
                name: "TvMazeId",
                table: "Person");
        }
    }
}
