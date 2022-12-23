using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redeo.Migrations
{
    /// <inheritdoc />
    public partial class OtherMovieRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movies_categories_moives_MovieId",
                table: "movies_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_moives",
                table: "moives");

            migrationBuilder.RenameTable(
                name: "moives",
                newName: "movies");

            migrationBuilder.AddColumn<string>(
                name: "Quality",
                table: "movies",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "movie_actors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_actors", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_movie_actors_actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movie_actors_movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movie_actors_ActorId",
                table: "movie_actors",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_movies_categories_movies_MovieId",
                table: "movies_categories",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movies_categories_movies_MovieId",
                table: "movies_categories");

            migrationBuilder.DropTable(
                name: "movie_actors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "movies");

            migrationBuilder.RenameTable(
                name: "movies",
                newName: "moives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_moives",
                table: "moives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_movies_categories_moives_MovieId",
                table: "movies_categories",
                column: "MovieId",
                principalTable: "moives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
