using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redeo.Migrations
{
    /// <inheritdoc />
    public partial class TvShowChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TvShowUrl",
                table: "tvShows");

            migrationBuilder.AddColumn<string>(
                name: "SeasonPoster",
                table: "seasons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EpisodeUrl",
                table: "episodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TvShowId",
                table: "episodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_episodes_TvShowId",
                table: "episodes",
                column: "TvShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_episodes_tvShows_TvShowId",
                table: "episodes",
                column: "TvShowId",
                principalTable: "tvShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_episodes_tvShows_TvShowId",
                table: "episodes");

            migrationBuilder.DropIndex(
                name: "IX_episodes_TvShowId",
                table: "episodes");

            migrationBuilder.DropColumn(
                name: "SeasonPoster",
                table: "seasons");

            migrationBuilder.DropColumn(
                name: "EpisodeUrl",
                table: "episodes");

            migrationBuilder.DropColumn(
                name: "TvShowId",
                table: "episodes");

            migrationBuilder.AddColumn<string>(
                name: "TvShowUrl",
                table: "tvShows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
