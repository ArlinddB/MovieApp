using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redeo.Migrations
{
    /// <inheritdoc />
    public partial class MoviePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoviePicture",
                table: "movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoviePicture",
                table: "movies");
        }
    }
}
