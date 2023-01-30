using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redeo.Migrations
{
    public partial class FavoriteTvShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteTvShows",
                columns: table => new
                {
                    TvShowId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isFavorite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteTvShows", x => new { x.TvShowId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FavoriteTvShows_tvShows_TvShowId",
                        column: x => x.TvShowId,
                        principalTable: "tvShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteTvShows_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "U_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteTvShows_UserId",
                table: "FavoriteTvShows",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteTvShows");
        }
    }
}
