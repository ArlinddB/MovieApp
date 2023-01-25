using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redeo.Migrations
{
    /// <inheritdoc />
    public partial class EpisodeDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EpisodeDescription",
                table: "episodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpisodeDescription",
                table: "episodes");
        }
    }
}
